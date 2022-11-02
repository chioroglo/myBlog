using System.ComponentModel.DataAnnotations;

namespace Common.Dto.Comment
{
    public class CommentDto
    {
        [Required]
        public int PostId { get; set; }

        [Required]
        public string Content { get; set; }
    }
}