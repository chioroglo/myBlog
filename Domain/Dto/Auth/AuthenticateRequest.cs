using System.ComponentModel.DataAnnotations;
using static Domain.Validation.EntityConfigurationConstants;

namespace Domain.Dto.Auth
{
    public class AuthenticateRequest
    {
        [Required]
        [MinLength(USER_USERNAME_MIN_LENGTH)]
        [MaxLength(USER_USERNAME_MAX_LENGTH)]
        public string Username { get; set; }

        [Required]
        [MinLength(USER_PASSWORD_MIN_LENGTH)]
        [MaxLength(USER_PASSWORD_MAX_LENGTH)]
        public string Password { get; set; }
    }
}