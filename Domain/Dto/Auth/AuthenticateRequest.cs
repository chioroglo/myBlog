using System.ComponentModel.DataAnnotations;
using static DAL.Configurations.EntityConfigurationConstants;

namespace Domain.Dto.Account
{
    public class AuthenticateRequest
    {
        [Required]
        [MinLength(USER_USERNAME_MIN_LENGTH)]
        [MaxLength(USER_USERNAME_MAX_LENGTH)]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}