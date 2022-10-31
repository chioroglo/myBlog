using Domain;
using System.ComponentModel.DataAnnotations;

namespace Common.Dto.PostReaction
{
    public class PostReactionDto
    {

        [Required]
        public int PostId { get; set; }

        [Required]
        [EnumDataType(typeof(ReactionType))]
        public ReactionType ReactionType { get; set; }
    }
}