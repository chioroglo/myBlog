using System.ComponentModel.DataAnnotations;
using static Common.Validation.EntityConfigurationConstants;

namespace Common.Dto.User
{
    public class UserInfoDto
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
    }
}
