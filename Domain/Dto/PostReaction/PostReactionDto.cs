namespace Domain.Dto.PostReaction
{
    public class PostReactionDto
    {
        public int UserId { get; set; }

        public int PostId { get; set; }

        public ReactionType ReactionType { get; set; }
    }
}
