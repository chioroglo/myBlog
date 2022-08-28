using Domain.Dto.Account;

namespace Service.Abstract.Auth
{
    public interface ITokenService
    {
        public Task<string> GenerateAccessToken(AuthenticateResponse userData);
    }
}
