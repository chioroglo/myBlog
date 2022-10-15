using Common.Dto.Paging.OffsetPaging;
using Domain;
using System.Linq.Expressions;

namespace Service.Abstract
{
    public interface IUserService : IEntityService<User>
    {
        Task<User> GetByUsernameAsync(string username, CancellationToken cancellationToken);

        Task<OffsetPagedResult<User>> GetOffsetPageAsync(OffsetPagedRequest query, CancellationToken cancellationToken, params Expression<Func<User, object>>[] includeProperties);
    }
}
