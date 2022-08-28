﻿using Domain.Dto.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Service.Abstract;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Service.Abstract.Auth;

namespace webApi.Controllers
{
    [Route("api/login")]
    public class LoginController : AppBaseController
    {
        private readonly IConfiguration _config;
        private readonly IAuthenticationService _authenticationService;
        private readonly ITokenService _tokenService;

        public LoginController(IConfiguration config,IAuthenticationService authenticationService,ITokenService tokenService)
        {
            _tokenService = tokenService;
            _authenticationService = authenticationService;
            _config = config;
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
