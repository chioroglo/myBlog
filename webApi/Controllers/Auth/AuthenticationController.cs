using API.Controllers.Base;
using Common.Dto.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract.Auth;

namespace API.Controllers.Auth
{
    [Route("api/login")]
    public class AuthenticationController : AppBaseController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ITokenService _tokenService;

        public AuthenticationController(IAuthenticationService authenticationService, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<AuthenticateResponse> LoginAsync([FromBody] AuthenticateRequest userData,CancellationToken cancellationToken)
        {
            var user = await _authenticationService.AuthenticateAsync(userData,cancellationToken);

            user.Token =  _tokenService.GenerateAccessToken(user);
            
            return user;
        }

    }
}