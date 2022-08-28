using System.ComponentModel.DataAnnotations;

namespace Domain.Dto.Post
{
    public class PostDto
    {
        public int? Id { get; set; }

        public int AuthorId { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
