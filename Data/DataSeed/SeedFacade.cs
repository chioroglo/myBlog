namespace DAL.DataSeed
{
    public static class SeedFacade
    {
        public static async Task SeedData(BlogDbContext dbContext)
        {
            await UserSeed.Seed(dbContext);
            await PostSeed.Seed(dbContext);
            await CommentSeed.Seed(dbContext);
            await PostReactionSeed.Seed(dbContext);
        }
    }
}