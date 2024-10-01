using Common.Dto.Auth;

namespace Common.Models.Passkey;

public class PasskeyAuthenticationOptionsModel
{
    public string Challenge { get; set; }
    public PasskeyRelyingPartyModel RelyingParty { get; set; }
}