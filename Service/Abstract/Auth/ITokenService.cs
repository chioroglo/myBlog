using Domain.Dto.Account;

namespace Service.Abstract.Auth
{
    public interface ITokenService
    {
        public string GenerateAccessToken(AuthenticateResponse userData);
    }
}
