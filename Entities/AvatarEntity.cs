using Entities.Abstract;

namespace Entities
{
    public class AvatarEntity : BaseEntity
    {
        public int UserId { get; set; }

        public UserEntity User { get; set; }

        public byte[] Avatar { get; set; }
    }
}