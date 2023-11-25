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
        private readonly IEncryptionService _encryptionService;

        public AuthenticationController(IAuthenticationService authenticationService, IEncryptionService encryptionService)
        {
            _encryptionService = encryptionService;
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<AuthenticateResponse> LoginAsync([FromBody] AuthenticateRequest userData,
            CancellationToken cancellationToken)
        {
            var authenticationResponse = await _authenticationService.AuthenticateAsync(userData, cancellationToken);
            authenticationResponse.Token = _encryptionService.GenerateAccessToken(authenticationResponse);

            return authenticationResponse;
        }
    }
}