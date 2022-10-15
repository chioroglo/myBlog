using Domain.Abstract;

namespace Domain
{
    public class Topic : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}
