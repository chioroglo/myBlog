using Domain.Validation.Attributes;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Domain.Dto.Avatar
{
    public class AvatarDto
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please select a file.")]
        [DataType(DataType.Upload)]
        [MaxAvatarFileSize(1024 * 5 * 1024)]
        [AllowedExtensions(new string[] {".jpg", ".png"})]
        public IFormFile Image { get; set; }
    }
}
