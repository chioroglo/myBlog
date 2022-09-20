using System.ComponentModel.DataAnnotations;

namespace Domain.Dto.PostReaction
{
    public class PostReactionDto
    {

        public int UserId { get; set; }
        
        [Required]
        public int PostId { get; set; }

        [Required]
        [EnumDataType(typeof(ReactionType))]
        public ReactionType ReactionType { get; set; }
    }
}