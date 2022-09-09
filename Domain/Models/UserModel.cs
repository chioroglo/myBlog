using Domain.Models.Abstract;

namespace Domain.Models
{
    public class UserModel : BaseModel
    {
        public string Username { get; set; }

        public string FullName { get; set; }

        public DateTime LastActivity { get; set; }
    }
}
