using AutoMapper;
using DAL.Repositories.Abstract;
using Domain;
using Domain.Models;
using Service.Abstract;
namespace Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task Add(User entity)
        {
            await _userRepository.Add(entity);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _userRepository.GetAll();
        }

        public async Task<User> GetById(int id)
        {
            return await _userRepository.GetById(id);
        }

        public async Task<bool> Remove(int id)
        {
            return await _userRepository.Remove(id);
        }

        public async Task<bool> Update(User entity)
        {
            try
            {
                await _userRepository.Update(entity);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
            return true;
        }

        public async Task<UserModel> GetByUsername(string username)
        {
            var usernameFound = await _userRepository.GetWhere(e => e.Username == username);

            if (usernameFound.Any())
            {
                return _mapper.Map<UserModel>(usernameFound.FirstOrDefault());
            }

            return null;
        }
    }
}
