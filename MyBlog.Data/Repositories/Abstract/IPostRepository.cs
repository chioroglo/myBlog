using MyBlog.Common.Models.Statistics;
using MyBlog.Data.Repositories.Abstract.Base;
using MyBlog.Domain;

namespace MyBlog.Data.Repositories.Abstract
{
    public interface IPostRepository : IBaseRepository<Post>
    {
        Task<Post?> GetByTitleAsync(string title, CancellationToken cancellationToken);
        Task<PostActivityModel> GetPostActivity(
            int postId,
            IEnumerable<DateTime> dates,
            TimeMeasure measure,
            CancellationToken ct);
    }
}