using MyBlog.Domain.Abstract;

namespace MyBlog.Domain;

public class UserWarning : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; }
    public string Reason { get; set; }
    public DateTime? RemovedAt { get; set; }
}