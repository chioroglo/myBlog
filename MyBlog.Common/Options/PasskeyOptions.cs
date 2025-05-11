using System.ComponentModel.DataAnnotations;

namespace MyBlog.Common.Options;

public class PasskeyOptions : BaseApplicationOptions
{
    public new static string Config => "PasskeyOptions";

    public PasskeyRelyingParty RelyingParty { get; set; }
    [Required]
    public TimeSpan ChallengeLifetime { get; set; }
}

public class PasskeyRelyingParty
{
    public string DomainName { get; init; }
    public string DisplayName { get; init; }
    public string Icon { get; init; }
    public IEnumerable<string> Origins { get; set; }
}