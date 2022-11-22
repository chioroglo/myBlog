using Domain.Abstract;

namespace Domain
{
    public class PostReaction : BaseEntity
    {
        public ReactionType ReactionType { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public int PostId { get; set; }

        public Post Post { get; set; }
    }
}