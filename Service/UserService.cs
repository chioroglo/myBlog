using DAL.Repositories.Abstract;
using Domain;
using Domain.Exceptions;
using Domain.Models.Pagination;
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
            _userRepository.Add(entity);
            await _userRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> GetById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                throw new ValidationException($"{nameof(User)} of ID: {id} does not exist");
            }

            return user;
        }

        public async Task<bool> Remove(int id, int issuerId)
        {
            if (id != issuerId)
            {
                throw new ValidationException($"{nameof(User)} of ID : {issuerId} cannot delete this account!");
            }

            await _userRepository.RemoveAsync(id);
            await _userRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Update(User entity)
        {
            _userRepository.Update(entity);            
            await _userRepository.SaveChangesAsync();

            return true;
        }

        public async Task<User?> GetByUsername(string username)
        {
            var usernameFound = await _userRepository.GetWhereAsync(e => e.Username == username);

            return usernameFound.FirstOrDefault();

        }

        public Task<PaginatedResult<User>> GetPage(PagedRequest query)
        {
            var pagedUsers = _userRepository.GetPagedData(query);
            
            return pagedUsers;
        }
    }
}
