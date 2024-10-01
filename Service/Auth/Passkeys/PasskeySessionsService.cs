using System.Text;
using Common.Dto.Auth;
using Common.Models.Passkey;
using Common.Options;
using Common.Utils;
using Domain;
using Fido2NetLib;
using Fido2NetLib.Objects;
using Microsoft.Extensions.Options;
using Service.Abstract;
using Service.Abstract.Auth.Passkeys;

namespace Service.Auth.Passkeys;

public class PasskeySessionsService : IPasskeySessionsService
{
    private readonly ICacheService _cache;
    private readonly IFido2 _fido2;
    private readonly PasskeyOptions _options;


    public PasskeySessionsService(
        ICacheService cache,
        IFido2 fido2,
        IOptions<PasskeyOptions> passkeyOptions)
    {
        _cache = cache;
        _fido2 = fido2;
        _options = passkeyOptions.Value;
    }

    public async Task<CredentialCreateOptions?> GetOngoingRegistrationSession(int userId, CancellationToken ct)
    {
        var cacheKey = PasskeyUtils.RegistrationCacheKey(userId);
        var session = await _cache.GetAsync<CredentialCreateOptions>(cacheKey);
        return session;
    }

    public async Task<CredentialCreateOptions> CreateRegistrationSession(User user, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(user);

        var cacheKey = PasskeyUtils.RegistrationCacheKey(user.Id);
        var fido2User = new Fido2User
        {
            Id = Encoding.UTF8.GetBytes(user.Id.ToString()),
            Name = user.Username,
            DisplayName = user.Username
        };

        var existingCredentialDescriptors = user.Passkeys
            .Select(p => new PublicKeyCredentialDescriptor(Encoding.UTF8.GetBytes(p.CredentialId))).ToList();
        var authenticatorSelection = new AuthenticatorSelection
        {
            UserVerification = UserVerificationRequirement.Required,
            RequireResidentKey = true
        };

        var fido2CredentialCreateOptions = _fido2.RequestNewCredential(
            fido2User,
            existingCredentialDescriptors,
            authenticatorSelection,
            AttestationConveyancePreference.Direct);

        await _cache.SetAsync(cacheKey, fido2CredentialCreateOptions, _options.ChallengeLifetime);
        return fido2CredentialCreateOptions;
    }

    public async Task RemoveOngoingRegistrationSession(int userId, CancellationToken ct)
    {
        var cacheKey = PasskeyUtils.RegistrationCacheKey(userId);
        await _cache.RemoveAsync(cacheKey);
    }

    public async Task<AssertionOptions?> GetOngoingAuthenticationSession(string challenge, CancellationToken ct)
    {
        var cacheKey = PasskeyUtils.AuthenticationCacheKey(challenge);
        var options = await _cache.GetAsync<AssertionOptions>(cacheKey);
        return options;
    }

    public async Task<PasskeyAuthenticationOptionsModel> CreateAuthenticationSession(CancellationToken ct)
    {
        var options = _fido2.GetAssertionOptions([], UserVerificationRequirement.Required);

        var challengeBase64 = Convert.ToBase64String(options.Challenge);
        var cacheKey = PasskeyUtils.AuthenticationCacheKey(challengeBase64);
        await _cache.SetAsync(cacheKey, options, _options.ChallengeLifetime);

        return new PasskeyAuthenticationOptionsModel
        {
            Challenge = challengeBase64,
            RelyingParty = new PasskeyRelyingPartyModel
            {
                Id = _options.RelyingParty.DomainName,
                Name = _options.RelyingParty.DisplayName,
                Icon = _options.RelyingParty.Icon
            }
        };
    }

    public async Task RemoveOngoingAuthenticationSession(string challenge, CancellationToken ct)
    {
        var cacheKey = PasskeyUtils.AuthenticationCacheKey(challenge);
        await _cache.RemoveAsync(cacheKey);
    }
}