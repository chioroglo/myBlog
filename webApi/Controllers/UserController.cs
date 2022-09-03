using API.Controllers.Base;
using AutoMapper;
using Domain;
using Domain.Dto.Account;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Service.Auth;
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
        public async Task<UserModel> GetAuthenticatedUser()
        {
            var currentId = Convert.ToInt32(HttpContext.User.FindFirst(TokenClaimNames.Id).Value);
            
            var user = await _userService.GetById(currentId);

            if (user != null)
            {
                return _mapper.Map<UserModel>(user);
            };

            return null;
        }
    }
}
