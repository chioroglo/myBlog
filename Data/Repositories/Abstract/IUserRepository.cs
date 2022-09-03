using DAL.Repositories.Abstract.Base;
using Entities;

namespace DAL.Repositories.Abstract
{
    public interface IUserRepository : IAsyncRepository<UserEntity>
    {

    }
}
