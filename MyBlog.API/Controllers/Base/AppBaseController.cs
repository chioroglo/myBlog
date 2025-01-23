using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MyBlog.Common;

namespace MyBlog.API.Controllers.Base
{
    [Authorize]
    [ApiController]
    public abstract class AppBaseController : ControllerBase
    {
        protected int CurrentUserId => Convert.ToInt32(HttpContext.User.FindFirstValue(TokenClaimNames.Id));
    }
}