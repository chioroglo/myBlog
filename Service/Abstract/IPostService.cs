using Domain;
using Domain.Models.Pagination;

namespace Service.Abstract
{
    public interface IPostService : IBaseService<Post>
    {
        Task<PaginatedResult<Post>> GetPage(PagedRequest query);
    }
}