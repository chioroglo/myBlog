using System.ComponentModel.DataAnnotations;
using static Common.Validation.EntityConfigurationConstants;

namespace Common.Dto.Post
{
    public class PostDto
    {
        [Required]
        [MaxLength(PostTitleMaxLength)]
        public string Title { get; set; }

        [Required] [MaxLength(PostMaxLength)] public string Content { get; set; }

        [MaxLength(MaxTopicNameLength)] public string? Topic { get; set; }
    }
}