using System.ComponentModel.DataAnnotations;
using static MyBlog.Common.Validation.EntityConfigurationConstants;

namespace MyBlog.Common.Dto.Comment
{
    public class CommentDto
    {
        [Required] public int PostId { get; set; }

        [MaxLength(CommentMaxLength)]
        [Required]
        public string Content { get; set; }
    }
}