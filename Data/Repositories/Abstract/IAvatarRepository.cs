using DAL.Repositories.Abstract.Base;
using Domain;

namespace DAL.Repositories.Abstract
{
    public interface IAvatarRepository : IAsyncRepository<Avatar>
    {
        Task<Avatar> GetByUserId(int userId);
    }
}
