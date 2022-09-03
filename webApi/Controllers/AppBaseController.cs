using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace webApi.Controllers
{
    [Authorize]
    [ApiController]
    public abstract class AppBaseController : ControllerBase
    {

    }
}
 