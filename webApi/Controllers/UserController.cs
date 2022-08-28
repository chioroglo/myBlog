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
        }
    }
}
