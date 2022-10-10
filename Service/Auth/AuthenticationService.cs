using AutoMapper;
using Common.Dto.Auth;
using DAL.Repositories.Abstract;
using Service.Abstract.Auth;
using System.Security.Authentication;

namespace Service.Auth
{
    public class AuthenticationService : IAuthenticationService
    {
        private IUserRepository _userRepository;
        private IMapper _mapper;

        public AuthenticationService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }


        public async Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest userData, CancellationToken cancellationToken)
        {
            var currentUser = await TryIdentifyUserAsync(userData.Username, userData.Password,cancellationToken)
                ?? throw new AuthenticationException("Credentials were not valid");
            
            return currentUser;
        }

        private async Task<AuthenticateResponse> TryIdentifyUserAsync(string username, string password, CancellationToken cancellationToken)
        {
            var matchingUsers = await _userRepository.GetWhereAsync(u => u.Username == username && u.Password == password,cancellationToken);

            if (!matchingUsers.Any())
            {
                return null;
            }

            var identifiedUser = matchingUsers.FirstOrDefault();

            var response = _mapper.Map<AuthenticateResponse>(identifiedUser);

            return response;
        }
    }
}