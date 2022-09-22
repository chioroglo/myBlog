using AutoMapper;
using Domain;
using Domain.Dto.Account;
using Domain.Models;
using Service.Abstract;
using Service.Abstract.Auth;
using System.ComponentModel.DataAnnotations;

namespace Service.Auth
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public RegistrationService(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<UserModel> Register(RegistrationDto registerData,CancellationToken cancellationToken)
        {
            
            if (await NicknameIsOccupied(registerData.Username,cancellationToken))
            {
                throw new ValidationException($"Username {registerData.Username} is occupied");
            }

            if (PasswordsDoNotMatch(registerData.Password,registerData.ConfirmPassword))
            {
                throw new ValidationException($"Passwords do not match");
            }

            User newUserEntity = _mapper.Map<User>(registerData);

            await _userService.Add(newUserEntity,cancellationToken);

            var newUserFromDb = await _userService.GetByUsername(registerData.Username,cancellationToken);

            return _mapper.Map<UserModel>(newUserFromDb);
        }

        private async Task<bool> NicknameIsOccupied(string username, CancellationToken cancellationToken)
        {
            User? requestOfUserOfProvidedNickname = await _userService.GetByUsername(username,cancellationToken);

            return requestOfUserOfProvidedNickname != null;
        }

        private bool PasswordsDoNotMatch(string actualPassword,string confirmationPassword)
        {
            return actualPassword != confirmationPassword;
        }
    }
}