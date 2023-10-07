using AutoMapper;
using Common.Dto.Auth;
using Common.Exceptions;
using DAL.Repositories.Abstract;
using Domain;
using Service.Abstract;
using Service.Abstract.Auth;

namespace Service.Auth
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IEncryptionService _encryptionService;

        public RegistrationService(IUserRepository userRepository, IMapper mapper, IEncryptionService encryptionService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _encryptionService = encryptionService;
        }

        public async Task<User> RegisterAsync(RegistrationDto registerData, CancellationToken cancellationToken)
        {
            if (await IsNicknameOccupied(registerData.Username, cancellationToken))
            {
                throw new ValidationException($"Username {registerData.Username} is occupied");
            }

            if (PasswordsDoNotMatch(registerData.Password, registerData.ConfirmPassword))
            {
                throw new ValidationException($"Passwords do not match");
            }

            var newUserEntity = _mapper.Map<User>(registerData);
            newUserEntity.PasswordHash = _encryptionService.EncryptPassword(newUserEntity.Password);
            
            await _userRepository.AddAsync(newUserEntity, cancellationToken);

            await _userRepository.SaveChangesAsync();

            var newlyCreatedUser =
                (await _userRepository.GetWhereAsync(u => u.Username == newUserEntity.Username, cancellationToken))
                .FirstOrDefault();

            return newlyCreatedUser;
        }

        private async Task<bool> IsNicknameOccupied(string username, CancellationToken cancellationToken)
        {
            var requestOfUserOfProvidedNickname =
                await _userRepository.GetWhereAsync(u => u.Username == username, cancellationToken);

            return requestOfUserOfProvidedNickname.Any();
        }

        private bool PasswordsDoNotMatch(string actualPassword, string confirmationPassword)
        {
            return actualPassword != confirmationPassword;
        }
    }
}