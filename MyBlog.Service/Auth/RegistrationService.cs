using AutoMapper;
using Microsoft.Extensions.Logging;
using MyBlog.Common.Dto.Auth;
using MyBlog.Common.Exceptions;
using MyBlog.Data.Repositories.Abstract;
using MyBlog.Domain;
using MyBlog.Domain.Abstract;
using MyBlog.Service.Abstract.Auth;

namespace MyBlog.Service.Auth
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IEncryptionService _encryptionService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RegistrationService> _logger;

        public RegistrationService(IUserRepository userRepository,
            IMapper mapper,
            IEncryptionService encryptionService,
            IUnitOfWork unitOfWork,
            ILogger<RegistrationService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _userRepository = userRepository;
            _mapper = mapper;
            _encryptionService = encryptionService;
        }

        public async Task<User> RegisterAsync(RegistrationDto registerData, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start registration attempt {@RegistrationAttempt}", registerData);
            if (await _userRepository.IsNicknameOccupied(registerData.Username, cancellationToken))
            {
                _logger.LogInformation("Username {Username} was occupied", registerData.Username);
                throw new ValidationException($"Username {registerData.Username} is occupied");
            }

            if (PasswordsDoNotMatch(registerData.Password, registerData.ConfirmPassword))
            {
                _logger.LogInformation("Passwords do not match");
                throw new ValidationException("Passwords do not match");
            }

            var newUserEntity = _mapper.Map<User>(registerData);
            newUserEntity.PasswordHash = _encryptionService.EncryptPassword(newUserEntity.Password);

            newUserEntity = await _userRepository.AddAsync(newUserEntity, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return newUserEntity;
        }

        private static bool PasswordsDoNotMatch(string actualPassword, string confirmationPassword)
        {
            return string.CompareOrdinal(actualPassword, confirmationPassword) != 0;
        }
    }
}