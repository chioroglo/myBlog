using MyBlog.Domain.Abstract;

namespace MyBlog.Domain;

public class UserBanLog : BaseEntity
{
    public string Reason { get; set; }
    public BanAction Action { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}

// TODO: convert to database table with scheme ENUM
public enum BanAction : short
{
    Ban = 0,
    Unban = 1
}