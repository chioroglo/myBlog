using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Service.Auth;
using System.Security.Claims;

namespace API.Controllers.Base
{
    [Authorize]
    [ApiController]
    public abstract class AppBaseController : ControllerBase
    {
        [NonAction]
        protected int GetCurrentUserId()
        {
            return Convert.ToInt32(HttpContext.User.FindFirstValue(TokenClaimNames.Id));
        }
    }
}
