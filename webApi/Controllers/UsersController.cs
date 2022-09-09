using API.Controllers.Base;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;
namespace webApi.Controllers
{
    [Route("api/users")]
    public class UsersController : AppBaseController
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _mapper = mapper;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IEnumerable<UserModel>> GetAllUsers()
        {
            var users = await _userService.GetAll();

            var userModels = users.Select(e => _mapper.Map<UserModel>(e));

            return userModels;
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetById(id);

            return user == null ? NotFound() : Ok(_mapper.Map<UserModel>(user));
        }

        [Route("[action]/{username}")]
        [HttpGet]
        public async Task<IActionResult> GetByUsername(string username)
        {
            var user = await _userService.GetByUsername(username);

            return user == null ? NotFound() : Ok(_mapper.Map<UserModel>(user));
        }

        [HttpGet("current")]
        public async Task<IActionResult> GetAuthenticatedUser()
        {
            var currentId = GetCurrentUserId();
            
            var user = await _userService.GetById(currentId);

            if (user != null)
            {
                return Ok(_mapper.Map<UserModel>(user));
            };

            return NotFound();
        }
    }
}