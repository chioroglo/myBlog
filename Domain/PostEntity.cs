using Domain.Abstract;

namespace Domain
{
    public class PostEntity : BaseEntity
    {
        public int UserId { get; set; }

        public UserEntity User { get; set; }

        public ICollection<CommentEntity> Comments { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
    }
}
