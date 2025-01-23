using MyBlog.Domain.Abstract;

namespace MyBlog.Domain
{
    public class Avatar : BaseEntity
    {
        public int UserId { get; set; }

        public User User { get; set; }

        public string BlobName { get; set; }
    }
}