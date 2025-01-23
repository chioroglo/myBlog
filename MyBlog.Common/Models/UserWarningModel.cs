using MyBlog.Common.Models.Abstract;

namespace MyBlog.Common.Models;

public class UserWarningModel : BaseModel
{
    public string Reason { get; set; }
    public DateTime? RemovedAt { get; set; }
}