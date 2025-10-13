using System.ComponentModel.DataAnnotations;

namespace MyBlog.Common.Options
{
    public class JsonWebTokenOptions : BaseApplicationOptions
    {
        public new static string Config => "Jwt";
        public required string PrivateKey { get; set; }
        public required string PublicKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        [Required]
        public TimeSpan AccessTokenValidityTime { get; set; }
        [Required]
        public TimeSpan RefreshTokenValidityTime { get; set; }
    }
}