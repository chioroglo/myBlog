using AutoMapper;
using DAL.Repositories.Abstract;
using Domain.Dto.Account;
using Entities;
using Microsoft.AspNetCore.Http;
using MyBlog.Domain.Dto.Auth;
using Service.Abstract;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IUserRepository userRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task Add(UserEntity entity)
        {
            await _userRepository.Add(entity);
        }

        public async Task<IEnumerable<UserEntity>> GetAll()
        {
            return await _userRepository.GetAll();
        }

        public async Task<UserEntity> GetById(int id)
        {
            return await _userRepository.GetById(id);
        }

        public async Task<AuthenticateResponse> GetCurrentUser()
        {
            Debug.WriteLine(_httpContextAccessor.HttpContext.User.FindFirst(TokenClaimNames.Username));
            Debug.WriteLine(_httpContextAccessor.HttpContext.User.FindFirst(TokenClaimNames.Id));

            var userId = Convert.ToInt32(_httpContextAccessor.HttpContext.User.FindFirstValue(TokenClaimNames.Id));
            var user = await _userRepository.GetById(userId);
            
            var response = _mapper.Map<AuthenticateResponse>(user);
            return response;
        }

        public async Task<IEnumerable<UserEntity>> GetWhere(Expression<Func<UserEntity, bool>> predicate)
        {
            return await _userRepository.GetWhere(predicate);
        }

        public async Task<AuthenticateResponse> TryIdentifyUser(string username, string password)
        {
            var matchingUsers = await _userRepository.GetWhere(u => u.Username == username && u.Password == password);
            
            if (!matchingUsers.Any())
            {
                return null;
            }

            var identifiedUser = matchingUsers.FirstOrDefault();

            var response = _mapper.Map<AuthenticateResponse>(identifiedUser);

            return response;
        }

        public async Task<bool> Remove(int id)
        {
            return await _userRepository.Remove(id);
        }

        public async Task Update(UserEntity entity)
        {
            await _userRepository.Update(entity);
        }
    }
}
