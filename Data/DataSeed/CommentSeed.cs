using Domain;

namespace DAL.DataSeed
{
    public class CommentSeed
    {
        public static async Task Seed(BlogDbContext dbContext)
        {
            if (!dbContext.Comments.Any())
            {
                var comment1 = new Comment()
                {
                    User = dbContext.Users.First(e => e.Username == "1937nkvd"),
                    Post = dbContext.Posts.First(e => e.Title == "Boone"),
                    Content = "this is post about ncr"
                };

                var comment2 = new Comment()
                {
                    User = dbContext.Users.First(e => e.Username == "1937nkvd"),
                    Post = dbContext.Posts.First(e => e.Title == "Boone"),
                    Content = "comment 2"
                };

                dbContext.Add(comment1);
                dbContext.Add(comment2);

                await dbContext.SaveChangesAsync();
            }
        }
    }
}