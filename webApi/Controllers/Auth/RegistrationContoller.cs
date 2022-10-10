using API.Controllers.Base;
using Common.Dto.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract.Auth;

namespace API.Controllers.Auth
{
    [Route("api/register")]
    public class RegistrationController : AppBaseController
    {
        private IRegistrationService _registrationService;

        public RegistrationController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task RegisterAsync([FromBody] RegistrationDto registrationData,CancellationToken cancellationToken)
        {
            await _registrationService.RegisterAsync(registrationData,cancellationToken);
        }
    }
}