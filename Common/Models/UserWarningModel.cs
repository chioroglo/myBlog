using Common.Models.Abstract;

namespace Common.Models;

public class UserWarningModel : BaseModel
{
    public string Reason { get; set; }
    public DateTime? RemovedAt { get; set; }
}