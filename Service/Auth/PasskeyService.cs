using System.Text;
using Common.Dto.Auth;
using Common.Exceptions;
using Common.Options;
using DAL.Repositories.Abstract;
using Domain;
using Domain.Passkey;
using Fido2NetLib;
using Fido2NetLib.Development;
using Fido2NetLib.Objects;
using Microsoft.Extensions.Options;
using Service.Abstract;
using Service.Abstract.Auth;

namespace Service.Auth;

public class PasskeyService : IPasskeyService
{
    private readonly IUserRepository _userRepository;
    private readonly IFido2 _fido2Core;
    private readonly IPasskeyRepository _passkeyRepository;
    private readonly PasskeyRelyingPartyOptions _passkeyOptions;
    private readonly ICacheService _cacheService;
    private readonly IEncryptionService _encryptionService;

    public PasskeyService(IUserRepository userRepository,
        IFido2 fido2Core,
        IPasskeyRepository passkeyRepository,
        IOptions<PasskeyRelyingPartyOptions> passkeyOptions,
        ICacheService cacheService,
        IEncryptionService encryptionService)
    {
        _userRepository = userRepository;
        _fido2Core = fido2Core;
        _passkeyRepository = passkeyRepository;
        _cacheService = cacheService;
        _encryptionService = encryptionService;
        _passkeyOptions = passkeyOptions.Value;
    }

    private static string CredentialCreateOptionsCacheKey(int userId)
    {
        return $"{nameof(CredentialCreateOptions)}_{userId}";
    }

    private static string AssertionOptionsCacheKey(int userId)
    {
        return $"{nameof(AssertionOptions)}_{userId}";
    }

    public async Task<CredentialCreateOptions> MakeCredentialOptions(int userId, CancellationToken ct)
    {
        var cacheKey = CredentialCreateOptionsCacheKey(userId);
        var user = await _userRepository.GetByIdAsync(userId, ct)
                   ?? throw new ValidationException($"{nameof(User)} of id {userId} does not exist");


        var existingPasskeys = await _passkeyRepository.GetWhereAsync(p => p.UserId == user.Id, ct);

        var userIdBytes = Encoding.UTF8.GetBytes(user.Id.ToString());

        var extensions = new AuthenticationExtensionsClientInputs
        {
            Extensions = false
        };

        var excludeCredentials =
            existingPasskeys.Select(p =>
                    new PublicKeyCredentialDescriptor(Encoding.UTF8.GetBytes(p.CredentialId))
                )
                .ToList();

        var options = _fido2Core.RequestNewCredential(new Fido2User
            {
                Id = userIdBytes,
                Name = user.Username,
                DisplayName = user.FullName ?? user.Username
            },
            excludeCredentials,
            AuthenticatorSelection.Default,
            AttestationConveyancePreference.None
        );

        await _cacheService.SetAsync(cacheKey, options);

        return options;
    }

    public async Task<Fido2.CredentialMakeResult> MakeCredential(AuthenticatorAttestationRawResponse dto, int userId,
        CancellationToken ct)
    {
        var cacheKey = CredentialCreateOptionsCacheKey(userId);
        
        try
        {
            var credentialCreateOptions = await _cacheService.GetAsync<CredentialCreateOptions>(cacheKey)
                                          ?? throw new Exception("no credential create options found");
            await _cacheService.RemoveAsync(cacheKey);

            var credentials = await _fido2Core.MakeNewCredentialAsync(
                dto,
                credentialCreateOptions,
                IsCredentialIdOccupied,
                cancellationToken: ct);

            if (credentials.Status != "ok" || credentials.Result == null) throw new Exception("not ok status");

            var success = credentials.Result;
            var credentialId = success.CredentialId;
            var publicKey = success.PublicKey;
            var counter = success.Counter;

            var passkey = new PasskeyStoredCredential
            {
                UserId = userId,
                CredentialId = Convert.ToBase64String(credentialId),
                PublicKey = Convert.ToBase64String(publicKey),
                UserHandle = Convert.ToBase64String(success.User.Id)
            };

            await _passkeyRepository.AddAsync(passkey, ct);

            return credentials;
        }
        finally
        {
        }
    }

    public async Task<AssertionOptions> GetAuthenticationOptionsAsync(int userId, CancellationToken ct)
    {
        var user = await _userRepository.GetByIdAsync(userId, ct)
                   ?? throw new ValidationException($"{nameof(User)} of id {userId} does not exist");

        var usedCredentials = await _passkeyRepository.GetWhereAsync(p => p.UserId == userId, ct);

        var options = _fido2Core.GetAssertionOptions(
            usedCredentials.Select(
                p => new PublicKeyCredentialDescriptor(
                    Encoding.UTF8.GetBytes(p.CredentialId)
                )
            ),
            UserVerificationRequirement.Discouraged);

        await _cacheService.SetAsync(AssertionOptionsCacheKey(userId), options);
        return options;
    }

    public async Task<AuthenticateResponse> Authenticate(int userId, AuthenticatorAssertionRawResponse dto,
        CancellationToken ct)
    {
        var cacheKey = AssertionOptionsCacheKey(userId);

        try
        {
            var assertionOptions = await _cacheService.GetAsync<AssertionOptions>(cacheKey);

            var passkey =
                await _passkeyRepository.GetWhereAsync(p => p.CredentialId == Convert.ToBase64String(dto.Id), ct);


            var res = await _fido2Core.MakeAssertionAsync(dto,
                assertionOptions,
                Encoding.UTF8.GetBytes(passkey.FirstOrDefault().PublicKey),
                0,
                DoesUserOwnCredentialId,
                cancellationToken: ct);

            if (res.Status != "ok") throw new ValidationException($"PasskeyStoredCredential verification failed for {userId}");

            var user = await _userRepository.GetByIdAsync(userId, ct);

            var authenticateResponse = new AuthenticateResponse
            {
                Id = userId,
                Username = user.Username
            };

            return authenticateResponse with
            {
                Token = _encryptionService.GenerateAccessToken(authenticateResponse)
            };
        }
        finally
        {
            await _cacheService.RemoveAsync(cacheKey);
        }
    }

    private async Task<bool> IsCredentialIdOccupied(IsCredentialIdUniqueToUserParams parameters, CancellationToken ct)
    {
        return !await _passkeyRepository.IsCredentialIdOccupied(Convert.ToBase64String(parameters.CredentialId), ct);
    }

    private async Task<bool> DoesUserOwnCredentialId(IsUserHandleOwnerOfCredentialIdParams parameters,
        CancellationToken ct)
    {
        var userId = Convert.ToBase64String(parameters.UserHandle);
        var credential = Convert.ToBase64String(parameters.CredentialId);
        return await _passkeyRepository.DoesUserOwnCredential(Convert.ToInt32(userId), credential, ct);
    }
}