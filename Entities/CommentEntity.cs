using Entities.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class CommentEntity : BaseEntity
    {
        public string Content { get; set; }

        public int AuthorId { get; set; }

        public int PostId { get; set; }
    }
}
