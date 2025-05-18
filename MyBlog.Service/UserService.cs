using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using MyBlog.Common.Exceptions;
using MyBlog.Common.Models;
using MyBlog.Data.Repositories.Abstract;
using MyBlog.Domain;
using MyBlog.Domain.Abstract;
using MyBlog.Service.Abstract;

namespace MyBlog.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasskeyRepository _passkeyRepository;
        private readonly IAvatarService _avatarService;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IAvatarService avatarService,
            IPasskeyRepository passkeyRepository,
            ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _avatarService = avatarService;
            _passkeyRepository = passkeyRepository;
            _logger = logger;
        }

        public async Task<User> Add(User entity, CancellationToken cancellationToken)
        {
            var user = await _userRepository.AddAsync(entity, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            _logger.LogInformation("Successfully created user {@User}", user);
            return user;
        }

        public async Task<User> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(id, cancellationToken);

            if (user == null)
            {
                throw new ValidationException($"{nameof(User)} of ID: {id} does not exist");
            }

            return user;
        }

        public async Task RemoveAsync(int id, int issuerId, CancellationToken cancellationToken)
        {
            if (id != issuerId)
            {
                throw new InsufficientPermissionsException(
                    $"{nameof(User)} of ID : {issuerId} cannot delete this account!");
            }

            await _userRepository.RemoveAsync(id, cancellationToken);
        }

        public async Task<User> UpdateAsync(User request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken) ??
                       throw new ValidationException($"User with ID {request.Id} was not found");

            if (!string.IsNullOrEmpty(request.Username))
            {
                var isNicknameOccupied = await _userRepository.IsNicknameOccupied(request.Username, cancellationToken);
                if (isNicknameOccupied)
                {
                    throw new ValidationException($"Username {request.Username} is occupied");
                }

                user.Username = request.Username;
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;

            await _unitOfWork.CommitAsync(cancellationToken);
            _logger.LogInformation("Personal data was updated successfully {@User}", user);

            return user;
        }

        public async Task<User> GetByIdWithIncludeAsync(int id, CancellationToken cancellationToken,
            params Expression<Func<User, object>>[] includeProperties)
        {
            var user = await _userRepository.GetByIdWithIncludeAsync(id, cancellationToken, includeProperties);

            return user ?? throw new ValidationException($"{nameof(User)} of ID: {id} does not exist");
        }

        public async Task UpdateLastActivity(int userId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
                       ?? throw new ValidationException($"{nameof(User)} of ID: {userId} does not exist");

            user.LastActivity = DateTime.UtcNow;
            await _unitOfWork.CommitAsync(cancellationToken);
        }

        public async Task<IEnumerable<Passkey>> GetActivePasskeys(int userId, CancellationToken cancellationToken)
        {
            var user = await _passkeyRepository.GetUserWithActivePasskeys(userId, cancellationToken)
                 ?? throw new NotFoundException($"{nameof(User)} of ID: {userId} does not exist");

            return user.Passkeys;
        }

        public async Task<User?> GetUserProfileData(int userId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetProfileData(userId, cancellationToken)
                       ?? throw new NotFoundException($"{nameof(User)} of ID: {userId} does not exist");

            return user;
        }

        public async Task<UserBadgeModel> GetBadge(int userId, CancellationToken cancellationToken)
        {
            var model = await _userRepository.GetBadge(userId, cancellationToken)
                ?? throw new NotFoundException($"User {userId} was not found");
            model.AvatarUrl = await _avatarService.GetAvatarUrlAsync(userId, cancellationToken);
            return model;
        }
    }
}