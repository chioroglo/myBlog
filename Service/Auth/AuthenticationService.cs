using AutoMapper;
using Common.Dto.Auth;
using DAL.Repositories.Abstract;
using Service.Abstract.Auth;
using System.Security.Authentication;
using Common;
using Common.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Service.Auth
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IEncryptionService _encryptionService;
        private readonly JsonWebTokenOptions _jwtOptions;

        public AuthenticationService(IUserRepository userRepository, IMapper mapper,
            IEncryptionService encryptionService, IOptions<JsonWebTokenOptions> jwtOptions)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _encryptionService = encryptionService;
            _jwtOptions = jwtOptions.Value;
        }


        public async Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest userData,
            CancellationToken cancellationToken)
        {
            var response = await TryIdentifyUserAsync(userData.Username, userData.Password, cancellationToken)
                           ?? throw new AuthenticationException("Credentials were not valid");

            response.AuthorizationExpirationDate = DateTime.UtcNow.AddMinutes(_jwtOptions.ValidityTimeMinutes);

            return response;
        }

        private async Task<AuthenticateResponse> TryIdentifyUserAsync(string username, string password,
            CancellationToken cancellationToken)
        {
            var hashedPassword = _encryptionService.EncryptPassword(password);
            var matchingUsers =
                await _userRepository.GetWhereAsync(u => u.Username == username && u.PasswordHash == hashedPassword,
                    cancellationToken);

            if (matchingUsers.IsNullOrEmpty())
            {
                return null;
            }

            var identifiedUser = matchingUsers.FirstOrDefault();

            var response = _mapper.Map<AuthenticateResponse>(identifiedUser);

            return response;
        }
    }
}