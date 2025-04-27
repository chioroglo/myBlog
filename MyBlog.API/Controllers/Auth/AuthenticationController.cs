using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.API.Controllers.Base;
using MyBlog.API.Extensions;
using MyBlog.API.Filters;
using MyBlog.Common.Dto.Auth;
using MyBlog.Common.Exceptions;
using MyBlog.Common.Models;
using MyBlog.Common.Utils;
using MyBlog.Service.Abstract.Auth;

namespace MyBlog.API.Controllers.Auth
{
    [Route("api/auth")]
    public class AuthenticationController : AppBaseController
    {
        private readonly IPasswordAuthService _passwordAuthService;
        private readonly Service.Abstract.Auth.IAuthorizationService _authorizationService;
        private readonly IMapper _mapper;
        private string? AccessToken => HttpContext.Request.Headers.Authorization
            .ToString()
            .Replace(JwtBearerDefaults.AuthenticationScheme, string.Empty)?.Trim();
        private string? RefreshToken => HttpContext?.Request.Cookies[JwtUtils.CookieRefreshTokenKey];


        public AuthenticationController(
            IPasswordAuthService passwordAuthService,
            Service.Abstract.Auth.IAuthorizationService authorizationService,
            IMapper mapper)
        {
            _passwordAuthService = passwordAuthService;
            _authorizationService = authorizationService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] PasswordAuthorizeRequest userData,
            CancellationToken cancellationToken)
        {
            var authenticationResponse = await _passwordAuthService.AuthenticateAsync(userData, cancellationToken);

            HttpContext.AddRefreshTokenCookie(authenticationResponse.RefreshToken, authenticationResponse.RefreshTokenExpiresAt);
            return Ok(_mapper.Map<AuthorizationResponseModel>(authenticationResponse));
        }

        [AllowAnonymous]
        [HttpGet("access-token/refresh")]
        public async Task<IActionResult> RefreshAccessToken(
            [FromQuery] [Required] int targetUserId,
            CancellationToken ct)
        {
            var refreshToken = RefreshToken ?? throw new AccessDeniedException("No refresh token set up");
            var newToken = await _authorizationService.GetNewAccessToken(refreshToken, targetUserId, ct);
            return Ok(new AuthorizationResponseModel
            {
                AccessToken = newToken,
                UserId = targetUserId
            });
        }

        [HttpPost("logout")]
        [UpdatesUserActivity]
        public async Task<IActionResult> Logout(CancellationToken ct)
        {
            var accessToken = AccessToken;

            await _authorizationService.PurgeRefreshToken(CurrentUserId, ct);
            await _authorizationService.BlacklistAccessToken(accessToken, ct);
            HttpContext.ClearRefreshToken();
            return Ok();
        }

        [HttpPatch("password/change")]
        [UpdatesUserActivity]
        public async Task<IActionResult> ChangePassword(
            [FromBody] ChangePasswordDto dto,
            CancellationToken ct = default)
        {
            dto.UserId = CurrentUserId;
            var result = await _passwordAuthService.ChangePasswordAsync(dto, ct);
            await _authorizationService.BlacklistAccessToken(AccessToken, ct);

            HttpContext.AddRefreshTokenCookie(result.RefreshToken, result.RefreshTokenExpiresAt);
            return Ok(_mapper.Map<AuthorizationResponseModel>(result));
        }
    }
}