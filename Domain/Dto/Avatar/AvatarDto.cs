using Domain.Validation.Attributes;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using static Domain.Validation.EntityConfigurationConstants;

namespace Domain.Dto.Avatar
{
    public class AvatarDto
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please select a file.")]
        [DataType(DataType.Upload)]
        [MaxAvatarFileSize(MaxAvatarSizeBytes)]
        [AllowedExtensions(new string[] {".jpg", ".png"})]
        public IFormFile Image { get; set; }
    }
}
