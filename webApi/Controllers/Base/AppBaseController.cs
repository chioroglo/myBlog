using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;
using System.Security.Claims;
using Common;

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

        protected int CurrentUserId => Convert.ToInt32(HttpContext.User.FindFirstValue(TokenClaimNames.Id));
    }
}