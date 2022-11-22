using Microsoft.EntityFrameworkCore;

namespace DAL.DataSeed
{
    public class SeedFacade
    {
        public static async Task SeedData(BlogDbContext dbContext)
        {
            await dbContext.Database.MigrateAsync();

            await UserSeed.Seed(dbContext);
            await PostSeed.Seed(dbContext);
            await CommentSeed.Seed(dbContext);
            await PostReactionSeed.Seed(dbContext);
        }
    }
}