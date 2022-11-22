using Domain.Abstract;

namespace Domain
{
    public class User : BaseEntity
    {
        public string Username { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string Password { get; set; }

        public DateTime LastActivity { get; set; }

        public Avatar Avatar { get; set; }

        public ICollection<Post> Posts { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<PostReaction> PostReactions { get; set; }
    }
}