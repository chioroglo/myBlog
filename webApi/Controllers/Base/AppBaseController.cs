using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Base
{
    [Authorize]
    [ApiController]
    public abstract class AppBaseController : ControllerBase
    {

    }
}
