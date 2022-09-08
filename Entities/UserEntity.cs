using Entities.Abstract;

namespace Entities
{
    public class UserEntity : BaseEntity
    {   public string Username { get; set; }

        public string? FirstName { get; set; }
        
        public string? LastName { get; set; }

        public string Password { get; set; }
        
        public DateTime LastActivity { get; set; }
        
        public AvatarEntity Avatar { get; set; }

        public ICollection<PostEntity> Posts { get; set; }

        public ICollection<CommentEntity> Comments { get; set; }
    }
}