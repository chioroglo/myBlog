using MyBlog.Common.Dto.Auth;
using MyBlog.Domain;

namespace MyBlog.Service.Abstract.Auth
{
    public interface IRegistrationService
    {
        public Task<User> RegisterAsync(RegistrationDto registerData, CancellationToken cancellationToken);
    }
}