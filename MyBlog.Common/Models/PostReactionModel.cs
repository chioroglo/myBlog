using MyBlog.Domain;

namespace MyBlog.Common.Models
{
    public class PostReactionModel
    {
        public ReactionType ReactionType { get; set; }

        public int UserId { get; set; }

        public int PostId { get; set; }
    }
}