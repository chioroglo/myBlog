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

        public async Task Add(User entity, CancellationToken cancellationToken)
        {
            await _userRepository.AddAsync(entity,cancellationToken);
            await _userRepository.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<User>> GetAll(CancellationToken cancellationToken)
        {
            return await _userRepository.GetAllAsync(cancellationToken);
        }

        public async Task<User> GetById(int id, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(id,cancellationToken);

            if (user == null)
            {
                throw new ValidationException($"{nameof(User)} of ID: {id} does not exist");
            }

            return user;
        }

        public async Task Remove(int id, int issuerId, CancellationToken cancellationToken)
        {
            if (id != issuerId)
            {
                throw new ValidationException($"{nameof(User)} of ID : {issuerId} cannot delete this account!");
            }

            await _userRepository.RemoveAsync(id,cancellationToken);
            await _userRepository.SaveChangesAsync(cancellationToken);
        }

        public async Task Update(User request, CancellationToken cancellationToken)
        {

            var user = await _userRepository.GetByIdAsync(request.Id,cancellationToken);

            if (request.Username != null)
            {
                if (await GetByUsername(request.Username, cancellationToken) != null)
                {
                    throw new ValidationException($"Username {request.Username} is occupied");
                }

                user.Username = request.Username;
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;

            _userRepository.Update(user,cancellationToken);
            await _userRepository.SaveChangesAsync(cancellationToken);
        }

        public async Task<User?> GetByUsername(string username, CancellationToken cancellationToken)
        {
            var usernameFound = await _userRepository.GetWhereAsync(e => e.Username == username,cancellationToken);

            return usernameFound.FirstOrDefault();
        }

        public Task<PaginatedResult<User>> GetPage(PagedRequest query, CancellationToken cancellationToken)
        {
            var pagedUsers = _userRepository.GetPagedData(query,cancellationToken);
            
            return pagedUsers;
        }
    }
}