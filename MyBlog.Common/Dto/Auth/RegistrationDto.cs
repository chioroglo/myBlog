using System.ComponentModel.DataAnnotations;
using MyBlog.Common.Logging;
using static MyBlog.Common.Validation.EntityConfigurationConstants;

namespace MyBlog.Common.Dto.Auth
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
        [LogPersonalData]
        public string? FirstName { get; set; }


        [MinLength(UserFirstAndLastNameMinLength)]
        [MaxLength(UserFirstAndLastNameMaxLength)]
        [RegularExpression(UserFirstnameAndLastnameRegEx)]
        [LogPersonalData]
        public string? LastName { get; set; }


        [Required]
        [MinLength(UserPasswordMinLength)]
        [MaxLength(UserPasswordMaxLength)]
        [LogSensitiveData]
        public string Password { get; set; }

        [Required]
        [MinLength(UserPasswordMinLength)]
        [MaxLength(UserPasswordMaxLength)]
        [LogSensitiveData]
        public string ConfirmPassword { get; set; }
    }
}