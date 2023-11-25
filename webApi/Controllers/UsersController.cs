﻿using API.Controllers.Base;
using API.Filters;
using AutoMapper;
using Common.Dto.User;
using Common.Models;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;

namespace API.Controllers
{
    [Route("api/users")]
    public class UsersController : AppBaseController
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }


        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<UserModel> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var user = await _userService.GetByIdAsync(id, cancellationToken);

            return _mapper.Map<UserModel>(user);
        }

        [AllowAnonymous]
        [HttpGet("username/{username}")]
        public async Task<UserModel> GetByUsernameAsync(string username, CancellationToken cancellationToken)
        {
            var user = await _userService.GetByUsernameAsync(username, cancellationToken);

            return _mapper.Map<UserModel>(user);
        }

        [HttpGet("current")]
        public async Task<UserModel> GetAuthenticatedUserAsync(CancellationToken cancellationToken)
        {
            var currentId = CurrentUserId;

            var user = await _userService.GetByIdAsync(currentId, cancellationToken);

            return _mapper.Map<UserModel>(user);
        }


        [HttpPatch]
        [UpdatesUserActivity]
        public async Task<UserModel> UpdateProfileInfoOfAuthenticatedUserAsync([FromBody] UserInfoDto newProfileInfo,
            CancellationToken cancellationToken)
        {
            var mappedRequest = _mapper.Map<User>(newProfileInfo);
            mappedRequest.Id = CurrentUserId;
            var updatedUser = await _userService.UpdateAsync(mappedRequest, cancellationToken);

            return _mapper.Map<UserModel>(updatedUser);
        }
    }
}