using Domain.Models.Abstract;

namespace Domain.Models
{
    public class CommentModel : BaseModel
    {
        public string Content { get; set; }

        public string AuthorUsername { get; set; }
    }
}