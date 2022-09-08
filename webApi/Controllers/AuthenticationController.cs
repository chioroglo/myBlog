using API.Controllers.Base;
using Domain.Dto.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract.Auth;

namespace webApi.Controllers
{
    [Route("api/login")]
    public class AuthenticationController : AppBaseController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ITokenService _tokenService;

        public AuthenticationController(IAuthenticationService authenticationService,ITokenService tokenService)
        {
            _tokenService = tokenService;
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] AuthenticateRequest userData)
        {
            var user = await _authenticationService.Authenticate(userData);

            if (user != null)   
            {
                var token = await _tokenService.GenerateAccessToken(user);
                return Ok(token);
            }

            return NotFound("Credentials were not valid");
        }

    }
}
