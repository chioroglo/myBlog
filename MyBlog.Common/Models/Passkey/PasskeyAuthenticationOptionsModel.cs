using MyBlog.Common.Dto.Auth;

namespace MyBlog.Common.Models.Passkey;

public class PasskeyAuthenticationOptionsModel
{
    public string Challenge { get; set; }
    public PasskeyRelyingPartyModel RelyingParty { get; set; }
}