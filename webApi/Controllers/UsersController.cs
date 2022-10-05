using API.Controllers.Base;
using AutoMapper;
using Domain;
using Domain.Dto.User;
using Domain.Models;
using Domain.Models.Pagination;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;
namespace API.Controllers
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

        [HttpGet("{id:int}")]
        public async Task<UserModel> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var user = await _userService.GetByIdAsync(id, cancellationToken);

            return _mapper.Map<UserModel>(user);
        }

        [HttpGet("username/{username}")]
        public async Task<UserModel> GetByUsernameAsync(string username, CancellationToken cancellationToken)
        {
            var user = await _userService.GetByUsernameAsync(username, cancellationToken);

            return _mapper.Map<UserModel>(user);
        }

        [HttpGet("current")]
        public async Task<UserModel> GetAuthenticatedUserAsync(CancellationToken cancellationToken)
        {
            var currentId = GetCurrentUserId();

            var user = await _userService.GetByIdAsync(currentId, cancellationToken);

            return _mapper.Map<UserModel>(user);
        }

        [HttpPost("paginated-search")]
        public async Task<PaginatedResult<UserModel>> GetPagedUsersAsync([FromBody] PagedRequest pagedRequest, CancellationToken cancellationToken)
        {
            var response = await _userService.GetPageAsync(pagedRequest, cancellationToken);

            return new PaginatedResult<UserModel>()
            {
                PageIndex = response.PageIndex,
                PageSize = response.PageSize,
                Total = response.Total,
                Items = response.Items.Select(e => _mapper.Map<UserModel>(e)).ToList()
            };
        }

        [HttpPatch]
        public async Task<UserModel> UpdateProfileInfoOfAuthenticatedUserAsync([FromBody] UserInfoDto newProfileInfo, CancellationToken cancellationToken)
        {
            var mappedRequest = _mapper.Map<User>(newProfileInfo);

            mappedRequest.Id = GetCurrentUserId();

            await _userService.UpdateAsync(mappedRequest, cancellationToken);

            var newUser = await _userService.GetByIdAsync(mappedRequest.Id, cancellationToken);

            return _mapper.Map<UserModel>(newUser);
        }

    }
}