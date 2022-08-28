using Entities.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class CommentEntity : BaseEntity
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public int AuthorId { get; set; }

        [Required]
        public int PostId { get; set; }
    }
}
