using Domain.Abstract;

namespace Domain
{
    public class CommentEntity : BaseEntity
    {
        public int UserId { get; set; }

        public UserEntity User { get; set; }

        public int PostId { get; set; }

        public PostEntity Post { get; set; }

        public string Content { get; set; }

    }
}
