using Entities.Abstract;

namespace Entities
{
    public class PostEntity : BaseEntity
    {
        public int AuthorId { get; set; }
        
        public string Content { get; set; }
    }
}
