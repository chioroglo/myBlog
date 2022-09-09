namespace Domain.Dto.Comment
{
    public class CommentDto
    {
        public int UserId { get; set; }

        public int PostId { get; set; }

        public string Content { get; set; }
    }
}