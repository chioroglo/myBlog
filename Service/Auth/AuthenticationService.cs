using Common.Dto.Auth;
using DAL.Repositories.Abstract;
using Service.Abstract.Auth;
using System.Security.Authentication;
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
            var response = await TryIdentifyUserAsync(userData.Username, userData.Password, cancellationToken)
                           ?? throw new AuthenticationException("Credentials were not valid");

            var authenticationResponse = _encryptionService.GenerateAccessToken(response.Id, response.Username);

            await _userService.UpdateLastActivity(response.Id, cancellationToken);
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