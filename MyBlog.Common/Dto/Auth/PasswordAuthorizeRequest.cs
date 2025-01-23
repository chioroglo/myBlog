using System.ComponentModel.DataAnnotations;
using static MyBlog.Common.Validation.EntityConfigurationConstants;

namespace MyBlog.Common.Dto.Auth
{
    public class PasswordAuthorizeRequest
    {
        [Required]
        [MinLength(UsernameMinLength)]
        [MaxLength(UsernameMaxLength)]
        [RegularExpression(UsernameRegEx)]
        public string Username { get; set; }

        [Required]
        [MinLength(UserPasswordMinLength)]
        [MaxLength(UserPasswordMaxLength)]
        public string Password { get; set; }
    }
}