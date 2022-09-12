using DAL.Repositories.Abstract;
using Domain;
using Domain.Exceptions;
using Service.Abstract;

namespace Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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

        public async Task<bool> Remove(int id, int issuerId)
        {
            if (id != issuerId)
            {
                throw new ValidationException($"You cannot delete this account!");
            }

            await _userRepository.RemoveAsync(id);
            await _userRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Update(User entity)
        {
            await _userRepository.UpdateAsync(entity);            
            await _userRepository.SaveChangesAsync();

            return true;
        }

        public async Task<User> GetByUsername(string username)
        {
            var usernameFound = await _userRepository.GetWhereAsync(e => e.Username == username);

            return usernameFound.FirstOrDefault();

        }
    }
}
