using Common.Dto.Auth;
using Fido2NetLib;
using Fido2NetLib.Objects;
using MyBlog.Common.Dto.Auth;
using MyBlog.Common.Exceptions;
using MyBlog.Domain;
using MyBlog.Service.Abstract.Auth.Passkeys;
using ResponseData = Fido2NetLib.AuthenticatorAttestationRawResponse.AttestationResponse;
using AssertionResponse = Fido2NetLib.AuthenticatorAssertionRawResponse.AssertionResponse;

namespace MyBlog.Service.Auth.Passkeys;

public class PasskeyCryptographyService(IFido2 fido2) : IPasskeyCryptographyService
{
    public async Task<Passkey> ValidateRegistration(RegisterPasskeyRequest request, User user,
        CredentialCreateOptions options, CancellationToken ct)
    {
        var rawId = Convert.FromBase64String(request.RawId);
        var attestationObject = Convert.FromBase64String(request.AttestationObject);
        var clientDataJson = Convert.FromBase64String(request.ClientDataJson);

        var fido2AttestationObject = new AuthenticatorAttestationRawResponse
        {
            Id = request.Id,
            RawId = rawId,
            Type = PublicKeyCredentialType.PublicKey,
            Response = new ResponseData
            {
                AttestationObject = attestationObject,
                ClientDataJson = clientDataJson
            }
        };

        try
        {
            var result = await fido2.MakeNewCredentialAsync(new MakeNewCredentialParams
            {
                AttestationResponse = fido2AttestationObject,
                OriginalOptions = options,
                IsCredentialIdUniqueToUserCallback = (newCredential, _) =>
                {
                    return Task.FromResult(user.Passkeys.All(p =>
                        !Convert.FromBase64String(p.CredentialId).SequenceEqual(newCredential.CredentialId)));
                }
            }, ct);

            return new Passkey
            {
                UserId = user.Id,
                CredentialId = Convert.ToBase64String(result.Id),
                PublicKey = Convert.ToBase64String(result.PublicKey),
                CredentialType = result.Type.ToString(),
                IsActive = true,
                AaGuid = result.AaGuid.ToString()
            };

        }
        catch (Exception ex)
        {
            throw new ValidationException(ex.Message);
        }

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
            Id = request.CredentialId,
            RawId = id,
            Response = assertionResponse,
            Type = PublicKeyCredentialType.PublicKey
        };


        // Throws on unsuccessful result, otherwise authentication is OK
        await fido2.MakeAssertionAsync(new MakeAssertionParams
        {
            AssertionResponse = authenticationAssertionObject,
            OriginalOptions = options,
            StoredPublicKey = publicKeyBase64,
            StoredSignatureCounter = 0,
            IsUserHandleOwnerOfCredentialIdCallback = (_, _) => Task.FromResult(true)
        }, ct);
   }
}
