using DAL.Repositories.Abstract.Base;
using Domain;

namespace DAL.Repositories.Abstract
{
    public interface IUserRepository : IAsyncRepository<UserEntity>
    {

    }
}
