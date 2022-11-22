using System.ComponentModel.DataAnnotations;
using static Common.Validation.EntityConfigurationConstants;

namespace Common.Dto.Comment
{
    public class CommentDto
    {
        [Required] public int PostId { get; set; }

        [MaxLength(CommentMaxLength)]
        [Required]
        public string Content { get; set; }
    }
}