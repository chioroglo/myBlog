using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static MyBlog.Common.Validation.EntityConfigurationConstants;

namespace MyBlog.Common.Dto.Auth;

public class ChangePasswordDto : IValidatableObject
{
    [Required]
    [MinLength(UserPasswordMinLength)]
    [MaxLength(UserPasswordMaxLength)]
    public string CurrentPassword { get; init; }
    [Required]
    [MinLength(UserPasswordMinLength)]
    [MaxLength(UserPasswordMaxLength)]
    public string NewPassword { get; init; }
    [Required]
    [MinLength(UserPasswordMinLength)]
    [MaxLength(UserPasswordMaxLength)]
    public string ConfirmNewPassword { get; init; }
    [JsonIgnore]
    public int UserId { get; set; } 

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.Compare(NewPassword, ConfirmNewPassword, StringComparison.InvariantCulture) != 0)
        {
            yield return new ValidationResult("Passwords do not match!", [NewPassword, ConfirmNewPassword]);
        }
    }
}