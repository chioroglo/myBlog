using Common.Dto.Auth;
using Common.Exceptions;
using Domain;
using Fido2NetLib;
using Fido2NetLib.Objects;
using Service.Abstract.Auth.Passkeys;
using ResponseData = Fido2NetLib.AuthenticatorAttestationRawResponse.ResponseData;
using AssertionResponse = Fido2NetLib.AuthenticatorAssertionRawResponse.AssertionResponse;
using CredentialMakeResult = Fido2NetLib.Fido2.CredentialMakeResult;


namespace Service.Auth.Passkeys;

public class PasskeyCryptographyService : IPasskeyCryptographyService
{
    private readonly IFido2 _fido2;

    public PasskeyCryptographyService(IFido2 fido2)
    {
        _fido2 = fido2;
    }

    public async Task<Passkey> ValidateRegistration(RegisterPasskeyRequest request, User user,
        CredentialCreateOptions options, CancellationToken ct)
    {
        var rawId = Convert.FromBase64String(request.RawId);
        var attestationObject = Convert.FromBase64String(request.AttestationObject);
        var clientDataJson = Convert.FromBase64String(request.ClientDataJson);

        var fido2AttestationObject = new AuthenticatorAttestationRawResponse
        {
            Id = rawId,
            RawId = rawId,
            Type = PublicKeyCredentialType.PublicKey,
            Response = new ResponseData
            {
                AttestationObject = attestationObject,
                ClientDataJson = clientDataJson
            }
        };

        var result = await _fido2.MakeNewCredentialAsync(
            fido2AttestationObject,
            options,
            isCredentialIdUniqueToUser: (newCredential, _) =>
            {
                return Task.FromResult(user.Passkeys.All(p =>
                    Convert.FromBase64String(p.CredentialId) != newCredential.CredentialId));
            },  cancellationToken: ct);

        if (result.Result == null ||
            !string.IsNullOrWhiteSpace(result.ErrorMessage) ||
            result.Status != "ok")
        {
            throw new ValidationException(result.Result?.ErrorMessage ?? "signature not valid");
        }

        return new Passkey
        {
            UserId = user.Id,
            CredentialId = Convert.ToBase64String(result.Result.CredentialId),
            PublicKey = Convert.ToBase64String(result.Result.PublicKey),
            CredentialType = result.Result.CredType,
            IsActive = true,
            AaGuid = result.Result.Aaguid.ToString()
        };
    }

    public async Task ValidateAuthentication(AuthenticatePasskeyRequest request, User user, AssertionOptions options, CancellationToken ct)
    {
        var targetPasskey = user.Passkeys
            .FirstOrDefault(p => 
                string.CompareOrdinal(p.CredentialId.Trim('='), request.CredentialId.Trim('=')) == 0);

        if (targetPasskey == null)
        {
            throw new ValidationException("Credential ID invalid!");
        }

        var id = Convert.FromBase64String(targetPasskey.CredentialId);
        var clientDataJson = Convert.FromBase64String(request.ClientDataJson);
        var signature = Convert.FromBase64String(request.Signature);
        var userHandle = Convert.FromBase64String(request.UserHandle);
        var authenticatorData = Convert.FromBase64String(request.AuthenticatorData);
        var publicKeyBase64 = Convert.FromBase64String(targetPasskey.PublicKey);

        var assertionResponse = new AssertionResponse
        {
            AuthenticatorData = authenticatorData,
            Signature = signature,
            ClientDataJson = clientDataJson,
            UserHandle = userHandle
        };

        var authenticationAssertionObject = new AuthenticatorAssertionRawResponse
        {
            Id = id,
            RawId = id,
            Response = assertionResponse,
            Type = PublicKeyCredentialType.PublicKey
        };


        // Throws on unsuccessful result, otherwise authentication is OK
        await _fido2.MakeAssertionAsync(
            authenticationAssertionObject,
            options,
            publicKeyBase64,
            0,
            async (_, _) => true,
            cancellationToken: ct);
   }
}