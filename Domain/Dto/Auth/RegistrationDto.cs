using System.ComponentModel.DataAnnotations;
using static DAL.Configurations.EntityConfigurationConstants;

namespace Domain.Dto.Account
{
    public class RegistrationDto
    {
        [MinLength(USER_USERNAME_MIN_LENGTH)]
        [MaxLength(USER_USERNAME_MAX_LENGTH)]
        public string Username { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        [MinLength(USER_PASSWORD_MIN_LENGTH)]
        [MaxLength(USER_USERNAME_MAX_LENGTH)]
        public string Password { get; set; }

        [MinLength(USER_PASSWORD_MIN_LENGTH)]
        [MaxLength(USER_USERNAME_MAX_LENGTH)]
        public string ConfirmPassword { get; set; }
    }
}
