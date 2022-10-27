using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Service.Auth;
using Service.Abstract;
using System.Security.Claims;

namespace API.Controllers.Base
{
    [Authorize]
    [ApiController]
    public abstract class AppBaseController : ControllerBase
    {
        protected readonly IUserService _userService;

        protected AppBaseController(IUserService userService)
        {
           _userService = userService;
        }

        [NonAction]
        protected int GetCurrentUserId()
        {
            return Convert.ToInt32(HttpContext.User.FindFirstValue(TokenClaimNames.Id));
        }

        protected async Task UpdateAuthorizedUserLastActivity(CancellationToken cancellationToken)
        {
            await _userService.UpdateLastActivity(GetCurrentUserId(), cancellationToken);
        }
    }
}
