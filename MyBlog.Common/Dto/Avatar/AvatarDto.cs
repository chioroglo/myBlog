using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using MyBlog.Common.Validation.Attributes;

namespace MyBlog.Common.Dto.Avatar
{
    public class AvatarDto
    {
        public int UserId { get; set; }

        // TODO add file size validation
        [Required(ErrorMessage = "Please select a file.")]
        [DataType(DataType.Upload)]
        [AllowedExtensions(new string[] { ".jpg", ".png" })]
        public IFormFile Image { get; set; }
    }
}