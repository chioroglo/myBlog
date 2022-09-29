using System.ComponentModel.DataAnnotations;
using static Domain.Validation.EntityConfigurationConstants;

namespace Domain.Dto.Account
{
    public class RegistrationDto
    {
        [MinLength(UsernameMinLength)]
        [MaxLength(UsernameMaxLength)]
        public string? Username { get; set; }

        [MinLength(UserFirstAndLastNameMinLength)]
        [MaxLength(UserFirstAndLastNameMaxLength)]
        public string? FirstName { get; set; }

        [MinLength(UserFirstAndLastNameMinLength)]
        [MaxLength(UserFirstAndLastNameMaxLength)]
        public string? LastName { get; set; }

        [MinLength(UserPasswordMinLength)]
        [MaxLength(UsernameMaxLength)]
        public string? Password { get; set; }

        [MinLength(UserPasswordMinLength)]
        [MaxLength(UsernameMaxLength)]
        public string? ConfirmPassword { get; set; }
    }
}
