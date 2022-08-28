using Domain.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class CommentModel : BaseModel
    {
        public string Content { get; set; }

        public string AuthorUsername { get; set; }

        byte[] AuthorAvatar { get; set; }
    }
}