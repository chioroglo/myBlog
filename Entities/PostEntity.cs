using Entities.Abstract;

namespace Entities
{
    public class PostEntity : BaseEntity
    {
        public int UserId { get; set; }

        public UserEntity User { get; set; }

        public ICollection<CommentEntity> Comments { get; set; }
        
        public string Content { get; set; }
    }
}
