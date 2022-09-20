using Domain.Abstract;

namespace Domain
{
    public class Post : BaseEntity
    {
        public int UserId { get; set; }

        public User User { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public ICollection<PostReaction> Reactions { get; set; }
    }
}