using Domain;

namespace DAL.DataSeed
{
    public class UserSeed
    {
        public static async Task Seed(BlogDbContext dbContext)
        {
            if (!dbContext.Users.Any())
            {
                var _1937nkvd = new User()
                {
                     Username = "1937nkvd",
                     Password = "qwerty",
                };

                var _vaflea = new User()
                {
                    Username = "vaflea",
                    Password = "lovemama123"
                };

                var _admin = new User()
                {
                    Username = "Admin",
                    Password = "admin"
                };

                dbContext.Add(_1937nkvd);
                dbContext.Add(_vaflea);
                dbContext.Add(_admin);

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
