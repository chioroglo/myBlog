using Common.Dto.Auth;

namespace Service.Abstract.Auth
{
    public interface IEncryptionService
    {
        public string GenerateAccessToken(AuthenticateResponse userData);
        public string EncryptPassword(string phrase);
    }
}