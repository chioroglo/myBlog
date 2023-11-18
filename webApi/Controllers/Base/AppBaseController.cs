using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Common;

namespace API.Controllers.Base
{
    [Authorize]
    [ApiController]
    public abstract class AppBaseController : ControllerBase
    {
        protected int CurrentUserId => Convert.ToInt32(HttpContext.User.FindFirstValue(TokenClaimNames.Id));
    }
}