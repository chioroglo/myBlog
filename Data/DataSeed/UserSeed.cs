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

                var _naomi = new User()
                {
                    Username = "vaflea",
                    Password = "lovemama123"
                };

                dbContext.Add(_1937nkvd);
                dbContext.Add(_naomi);

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
