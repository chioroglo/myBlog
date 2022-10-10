using Common.Dto.Auth;

namespace Service.Abstract.Auth
{
    public interface ITokenService
    {
        public string GenerateAccessToken(AuthenticateResponse userData);
    }
}
