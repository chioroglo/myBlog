using Domain.Models.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class PostModel : BaseModel
    {
        [Required]
        public int AuthorId { get; set; }

        [Required]
        public string AuthorUsername { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }
}