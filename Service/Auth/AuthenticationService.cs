using Domain.Dto.Account;
using Service.Abstract;
using Service.Abstract.Auth;

namespace Service.Auth
{
    public class AuthenticationService : IAuthenticationService
    {
        private IUserService _userService;

        public AuthenticationService(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest userData)
        {
            var currentUser = await _userService.IdentifyUser(userData.Username, userData.Password);
            return currentUser;
        }
    }
}
