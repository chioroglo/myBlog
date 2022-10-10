using Common.Models.Abstract;

namespace Common.Models
{
    public class PostModel : BaseModel
    {
        public int AuthorId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
    }
}