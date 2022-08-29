using Entities.Abstract;

namespace Entities
{
    public class UserEntity : BaseEntity
    {   public string Username { get; set; }

        public string? FirstName { get; set; }
        
        public string? LastName { get; set; }

        public string Password { get; set; }

        public byte[]? Avatar { get; set; }

        public DateTime LastActivity { get; set; }
    }
}