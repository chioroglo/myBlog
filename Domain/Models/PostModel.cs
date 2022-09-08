using Domain.Models.Abstract;

namespace Domain.Models
{
    public class PostModel : BaseModel
    {
        public int AuthorId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
    }
}
