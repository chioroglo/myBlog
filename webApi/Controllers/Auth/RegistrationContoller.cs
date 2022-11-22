using API.Controllers.Base;
using AutoMapper;
using Common.Dto.Auth;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;
using Service.Abstract.Auth;

namespace API.Controllers.Auth
{
    [Route("api/register")]
    public class RegistrationController : AppBaseController
    {
        private readonly IRegistrationService _registrationService;
        private readonly IMapper _mapper;

        public RegistrationController(IRegistrationService registrationService, IMapper mapper,
            IUserService userService) : base(userService)
        {
            _registrationService = registrationService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<UserModel> RegisterAsync([FromBody] RegistrationDto registrationData,
            CancellationToken cancellationToken)
        {
            var user = await _registrationService.RegisterAsync(registrationData, cancellationToken);

            return _mapper.Map<UserModel>(user);
        }
    }
}