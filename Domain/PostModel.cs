using Domain.Abstract;

namespace Domain
{
    public class PostModel : BaseModel
    {
        public int AuthorId { get; set; }

        public string Content { get; set; }

        public string AuthorUsername { get; set; }
        
        public byte[] AuthorAvatar { get; set; }

        public List<CommentModel> Comments { get; set; }
    }
}
