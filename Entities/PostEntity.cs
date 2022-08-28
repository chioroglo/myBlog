using Entities.Abstract;
using System.ComponentModel.DataAnnotations;
using static Entities.DatabaseConfigurationConstants;

namespace Entities
{
    public class PostEntity : BaseEntity
    {
        [Required]
        public int AuthorId { get; set; }
        
        [Required]
        [MaxLength(POST_MAX_LENGTH)]
        public string Content { get; set; }
    }
}
