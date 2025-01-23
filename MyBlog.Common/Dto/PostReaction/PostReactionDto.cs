using System.ComponentModel.DataAnnotations;
using MyBlog.Domain;

namespace MyBlog.Common.Dto.PostReaction
{
    public class PostReactionDto
    {
        [Required] public int PostId { get; set; }

        [Required]
        [EnumDataType(typeof(ReactionType))]
        public ReactionType ReactionType { get; set; }
    }
}