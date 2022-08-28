using AutoMapper;
using Domain.Dto.Account;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;

namespace webApi.Controllers
{
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
        }
    }
}
