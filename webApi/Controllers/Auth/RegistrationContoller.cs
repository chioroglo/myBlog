using API.Controllers.Base;
using AutoMapper;
using Common.Dto.Auth;
using Common.Models;
using Fido2NetLib;
using Fido2NetLib.Development;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract.Auth;

namespace API.Controllers.Auth
{
    [Route("api/register")]
    public class RegistrationController : AppBaseController
    {
        private readonly IRegistrationService _registrationService;
        private readonly IPasskeyService _passkeyService;
        private readonly IMapper _mapper;

        public RegistrationController(IRegistrationService registrationService,
            IMapper mapper,
            IPasskeyService passkeyService)
        {
            _registrationService = registrationService;
            _mapper = mapper;
            _passkeyService = passkeyService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<UserModel> RegisterAsync([FromBody] RegistrationDto registrationData,
            CancellationToken cancellationToken)
        {
            var user = await _registrationService.RegisterAsync(registrationData, cancellationToken);

            return _mapper.Map<UserModel>(user);
        }

        [HttpPost("makeCredentialOptions")]
        public async Task<CredentialCreateOptions> MakeCredentialOptions(CancellationToken ct)
        {
            var response = await _passkeyService.GetCredentialOptionsAsync(CurrentUserId, ct);
            return response;
        }

        [HttpPost("makeCredential")]
        public async Task<Fido2.CredentialMakeResult> MakeCredential([FromBody] AuthenticatorAttestationRawResponse attestationResponse, CancellationToken ct)
        {
            var response = await _passkeyService.MakeCredential(attestationResponse, CurrentUserId, ct);

            return response;
        }

        [AllowAnonymous]
        [HttpGet("assertionOptions")]
        public async Task<AssertionOptions> GetLoginOptions([FromQuery] int userId, CancellationToken ct)
        {
            var options = await _passkeyService.GetAuthenticationOptionsAsync(userId, ct);
            return options;
        }

        [AllowAnonymous]
        [HttpPost("makeAssertion")]
        public async Task<AuthenticateResponse> Login([FromRoute] int userId,AuthenticatorAssertionRawResponse rawResponse, CancellationToken ct)
        {
            var result = await _passkeyService.Authenticate(userId, rawResponse, ct);
            return result ;
        }
    }
}