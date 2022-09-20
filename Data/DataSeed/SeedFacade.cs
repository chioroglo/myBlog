using Microsoft.EntityFrameworkCore;

namespace DAL.DataSeed
{
    public class SeedFacade
    {
        public static async Task SeedData(BlogDbContext dbContext)
        {
            dbContext.Database.Migrate();

            await UserSeed.Seed(dbContext);
            await PostSeed.Seed(dbContext);
            await CommentSeed.Seed(dbContext);
            await PostReactionSeed.Seed(dbContext);
        }
    }
}
