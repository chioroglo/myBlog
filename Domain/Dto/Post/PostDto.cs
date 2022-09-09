using System.ComponentModel.DataAnnotations;

namespace Domain.Dto.Post
{
    public class PostDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
