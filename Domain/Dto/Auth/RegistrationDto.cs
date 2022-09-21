using System.ComponentModel.DataAnnotations;
using static Domain.Validation.EntityConfigurationConstants;

namespace Domain.Dto.Account
{
    public class RegistrationDto
    {
        [MinLength(USER_USERNAME_MIN_LENGTH)]
        [MaxLength(USER_USERNAME_MAX_LENGTH)]
        public string Username { get; set; }

        [MinLength(USER_FIRSTNAME_LASTNAME_MIN_LENGTH)]
        [MaxLength(USER_FIRSTNAME_LASTNAME_MAX_LENGTH)]
        public string? FirstName { get; set; }

        [MinLength(USER_FIRSTNAME_LASTNAME_MIN_LENGTH)]
        [MaxLength(USER_FIRSTNAME_LASTNAME_MAX_LENGTH)]
        public string? LastName { get; set; }

        [MinLength(USER_PASSWORD_MIN_LENGTH)]
        [MaxLength(USER_USERNAME_MAX_LENGTH)]
        public string Password { get; set; }

        [MinLength(USER_PASSWORD_MIN_LENGTH)]
        [MaxLength(USER_USERNAME_MAX_LENGTH)]
        public string ConfirmPassword { get; set; }
    }
}
