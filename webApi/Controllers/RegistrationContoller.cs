using Domain;
using Domain.Dto.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract.Auth;
using webApi.Controllers;

namespace API.Controllers
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
        public async Task<IActionResult> Register([FromBody] RegistrationDto registrationData)
        {
            UserModel response;
            try
            {
                response = await _registrationService.Register(registrationData);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(response);
        }
    }
}
