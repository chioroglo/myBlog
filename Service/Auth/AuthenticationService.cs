using Common.Dto.Auth;
using DAL.Repositories.Abstract;
using Service.Abstract.Auth;
using System.Security.Authentication;
using Common.Exceptions;
using Domain;
using Service.Abstract;

namespace Service.Auth
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly IUserService _userService;

        public AuthenticationService(
            IUserRepository userRepository,
            IEncryptionService encryptionService,
            IUserService userService)
        {
            _userRepository = userRepository;
            _encryptionService = encryptionService;
            _userService = userService;
        }

        public async Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest userData,
            CancellationToken cancellationToken)
        {
            var user = await TryIdentifyUserAsync(userData.Username, userData.Password, cancellationToken)
                           ?? throw new ValidationException("Credentials were not valid");

            if (user.IsBanned)
            {
                throw new ValidationException($"User is banned! Please contact platform administrators!");
            }

            var authenticationResponse = _encryptionService.GenerateAccessToken(user.Id, user.Username);

            await _userService.UpdateLastActivity(user.Id, cancellationToken);
            return authenticationResponse;
        }

        private async Task<User?> TryIdentifyUserAsync(string username, string password,
            CancellationToken cancellationToken)
        {
            var hashedPassword = _encryptionService.EncryptPassword(password);
            var matchingUsers =
                await _userRepository.GetWhereAsync(u => u.Username == username && u.PasswordHash == hashedPassword,
                    cancellationToken);

            return matchingUsers?.FirstOrDefault();
        }
    }
}