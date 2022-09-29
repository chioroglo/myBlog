using System.ComponentModel.DataAnnotations;
using static Domain.Validation.EntityConfigurationConstants;

namespace Domain.Dto.Auth
{
    public class AuthenticateRequest
    {
        [Required]
        [MinLength(UsernameMinLength)]
        [MaxLength(UsernameMaxLength)]
        public string Username { get; set; }

        [Required]
        [MinLength(UserPasswordMinLength)]
        [MaxLength(UserPasswordMaxLength)]
        public string Password { get; set; }
    }
}