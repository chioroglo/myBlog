
using Domain;

namespace DAL.DataSeed
{
    public class PostReactionSeed
    {
        public static async Task Seed(BlogDbContext dbContext)
        {
            if (!dbContext.PostReactions.Any())
            {
                var reaction1 = new PostReaction()
                {
                    User = dbContext.Users.First(e => e.Username == "vaflea"),
                    Post = dbContext.Posts.First(e => e.Title == "Boone"),
                    ReactionType = ReactionType.Like
                };

                var reaction2 = new PostReaction()
                {
                    User = dbContext.Users.First(e => e.Username == "admin"),
                    Post = dbContext.Posts.First(e => e.Title == "Boone"),
                    ReactionType = ReactionType.Like
                };

                var reaction3 = new PostReaction()
                {
                    User = dbContext.Users.First(e => e.Username == "1937nkvd"),
                    Post = dbContext.Posts.First(e => e.Title == "Boone"),
                    ReactionType = ReactionType.Love
                };

                dbContext.Add(reaction1);
                dbContext.Add(reaction2);
                dbContext.Add(reaction3);

                await dbContext.SaveChangesAsync();
            }
        }
    }
}