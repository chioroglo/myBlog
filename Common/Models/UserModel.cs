using Common.Models.Abstract;

namespace Common.Models
{
    public class UserModel : BaseModel
    {
        public string Username { get; set; }

        public string FullName { get; set; }

        public string LastActivity { get; set; }
    }
}
