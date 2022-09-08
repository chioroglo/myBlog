using Domain.Models.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class CommentModel : BaseModel
    {
        public string Content { get; set; }

        public string AuthorUsername { get; set; }

        byte[] AuthorAvatar { get; set; }
    }
}