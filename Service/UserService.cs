using AutoMapper;
using DAL.Repositories.Abstract;
using Domain;
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
            await _userRepository.AddAsync(entity);
            await _userRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> GetById(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<bool> Remove(int id)
        {
            var wasSuccessfullyDeleted = await _userRepository.RemoveAsync(id);
            await _userRepository.SaveChangesAsync();

            return wasSuccessfullyDeleted;
        }

        public async Task<bool> Update(User entity)
        {
            try
            {
                await _userRepository.UpdateAsync(entity);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
            
            await _userRepository.SaveChangesAsync();
            return true;
        }

        public async Task<User> GetByUsername(string username)
        {
            var usernameFound = await _userRepository.GetWhereAsync(e => e.Username == username);

            if (usernameFound.Any())
            {
                return usernameFound.FirstOrDefault();
            }
            return null;
        }
    }
}
