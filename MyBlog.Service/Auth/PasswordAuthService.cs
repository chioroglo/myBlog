using Microsoft.Identity.Client;
using MyBlog.Common.Dto.Auth;
using MyBlog.Common.Exceptions;
using MyBlog.Data.Repositories.Abstract;
using MyBlog.Domain.Abstract;
using MyBlog.Service.Abstract.Auth;

namespace MyBlog.Service.Auth
{
    public class PasswordAuthService : IPasswordAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUnitOfWork _unitOfWork;

        public PasswordAuthService(
            IUserRepository userRepository,
            IEncryptionService encryptionService,
            IAuthorizationService authorizationService,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _encryptionService = encryptionService;
            _authorizationService = authorizationService;
            _unitOfWork = unitOfWork;
        }

        public async Task<AuthorizationResponse> AuthenticateAsync(PasswordAuthorizeRequest userData,
            CancellationToken cancellationToken)
        {
            var hashedPassword = _encryptionService.EncryptPassword(userData.Password);
            var user = await _userRepository.GetUserByCredentials(userData.Username, hashedPassword, cancellationToken)
                       ?? throw new ValidationException("Credentials were not valid");

            return await _authorizationService.Authorize(user, cancellationToken);
        }

        public async Task ChangePasswordAsync(ChangePasswordDto dto, CancellationToken ct = default)
        {
            var user = await _userRepository.GetByIdAsync(dto.UserId, ct)
                ?? throw new ValidationException($"User {dto.UserId} was not found");

            var currentPasswordHash = _encryptionService.EncryptPassword(dto.CurrentPassword);

            if (string.Compare(currentPasswordHash, user.PasswordHash, StringComparison.InvariantCulture) != 0)
            {
                throw new ValidationException("Current password is not valid");
            }

            user.PasswordHash = _encryptionService.EncryptPassword(dto.NewPassword);
            await _unitOfWork.CommitAsync(ct);
        }
    }
}