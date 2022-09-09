using AutoMapper;
using DAL.Repositories.Abstract;
using Domain.Dto.Account;
using Domain.Dto.Auth;
using Service.Abstract;
using Service.Abstract.Auth;

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

        public async Task<AuthenticateResponse> TryIdentifyUser(string username, string password)
        {
            var matchingUsers = await _userRepository.GetWhereAsync(u => u.Username == username && u.Password == password);

            if (!matchingUsers.Any())
            {
                return null;
            }

            var identifiedUser = matchingUsers.FirstOrDefault();

            var response = _mapper.Map<AuthenticateResponse>(identifiedUser);

            return response;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest userData)
        {
            var currentUser = await TryIdentifyUser(userData.Username, userData.Password);
            return currentUser;
        }
    }
}
