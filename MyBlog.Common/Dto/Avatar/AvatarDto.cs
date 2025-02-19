using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using MyBlog.Common.Validation.Attributes;
using static MyBlog.Common.Validation.EntityConfigurationConstants;

namespace MyBlog.Common.Dto.Avatar
{
    public class AvatarDto : IValidatableObject
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please select a file.")]
        [DataType(DataType.Upload)]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".jpeg" })]
        public IFormFile Image { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Image?.Length > MaxAvatarSizeKb * 1024)
            {
                yield return new ValidationResult($"File size is above {MaxAvatarSizeKb} KB", ["Image"]);
            }
        }
    }
}