using System.ComponentModel.DataAnnotations;

namespace Common.Dto.Post
{
    public class PostDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public string? Topic { get; set; }
    }
}
