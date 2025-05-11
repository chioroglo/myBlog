using System.ComponentModel.DataAnnotations;

namespace MyBlog.Common.Options
{
    public class JsonWebTokenOptions : BaseApplicationOptions
    {
        public new static string Config => "Jwt";
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        [Required]
        public TimeSpan AccessTokenValidityTime { get; set; }
        [Required]
        public TimeSpan RefreshTokenValidityTime { get; set; }
    }
}