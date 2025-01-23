using MyBlog.Domain;

namespace MyBlog.Data.DataSeed
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
                    PasswordHash = "65e84be33532fb784c48129675f9eff3a682b27168c0ea744b2cf58ee02337c5" // qwerty
                };

                var @vaflea = new User()
                {
                    Username = "vaflea",
                    PasswordHash = "9bcd53793c43361386708990a5a7827140deb591910da5fd8649a9b81759ffa6" // lovemama123
                };

                var @admin = new User()
                {
                    Username = "Admin",
                    PasswordHash = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918" // admin
                };

                dbContext.AddRange(new List<User> { _1937nkvd, vaflea, admin });

                await dbContext.SaveChangesAsync();
            }
        }
    }
}