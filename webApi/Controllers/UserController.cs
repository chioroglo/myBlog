using AutoMapper;
using Domain;
using Domain.Dto.Account;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;
using System.Security.Claims;

namespace webApi.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : AppBaseController
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _mapper = mapper;
            _userService = userService;
        }

        [HttpGet("current")]
        public async Task<AuthenticateResponse> GetAuthenticatedUser()
        {
            var user = await _userService.GetCurrentUser();

            if (user != null)
            {
                return user;
            };

            return null;
            /*
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var identityClaims = identity.Claims;

                return new AuthenticateResponse()
                {
                    Id = Convert.ToInt32(identityClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value),
                    Username = identityClaims.FirstOrDefault(o => o.Type == ClaimTypes.Name)?.Value
                };
            }
            return null;
            */
        }
    }
}
