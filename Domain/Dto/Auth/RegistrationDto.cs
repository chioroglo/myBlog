using System.ComponentModel.DataAnnotations;
using static Domain.Validation.EntityConfigurationConstants;

namespace Domain.Dto.Account
{
    public class RegistrationDto
    {

        [Required]
        [MinLength(UsernameMinLength)]
        [MaxLength(UsernameMaxLength)]
        [RegularExpression(UsernameRegEx)]
        public string Username { get; set; }

        [MinLength(UserFirstAndLastNameMinLength)]
        [MaxLength(UserFirstAndLastNameMaxLength)]
        [RegularExpression(UserFirstnameAndLastnameRegEx)]
        public string? FirstName { get; set; }


        [MinLength(UserFirstAndLastNameMinLength)]
        [MaxLength(UserFirstAndLastNameMaxLength)]
        [RegularExpression(UserFirstnameAndLastnameRegEx)]
        public string? LastName { get; set; }


        [Required]
        [MinLength(UserPasswordMinLength)]
        [MaxLength(UsernameMaxLength)]
        public string Password { get; set; }

        [Required]
        [MinLength(UserPasswordMinLength)]
        [MaxLength(UsernameMaxLength)]
        public string ConfirmPassword { get; set; }
    }
}
