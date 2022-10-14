using System.ComponentModel.DataAnnotations;
using static Common.Validation.EntityConfigurationConstants;

namespace Common.Dto.Auth
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
        [MaxLength(UserPasswordMaxLength)]
        public string Password { get; set; }

        [Required]
        [MinLength(UserPasswordMinLength)]
        [MaxLength(UsernameMaxLength)]
        public string ConfirmPassword { get; set; }
    }
}
