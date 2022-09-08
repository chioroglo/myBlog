using AutoMapper;
using Domain;
using Domain.Dto.Account;
using Domain.Models;
using Service.Abstract;
using Service.Abstract.Auth;

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

        public async Task<UserModel> Register(RegistrationDto registerData)
        {
            
            if (await NicknameIsOccupied(registerData.Username))
            {
                throw new Exception($"Username {registerData.Username} is occupied");
            }
            if (PasswordsDoNotMatch(registerData.Password,registerData.ConfirmPassword))
            {
                throw new Exception($"Passwords do not match");
            }

            UserEntity newUserEntity = _mapper.Map<UserEntity>(registerData);

            await _userService.Add(newUserEntity);

            return await _userService.GetByUsername(registerData.Username);
        }

        private async Task<bool> NicknameIsOccupied(string username)
        {
            UserModel occupiedNicknames = await _userService.GetByUsername(username);

            return occupiedNicknames != null;
        }

        private bool PasswordsDoNotMatch(string actualPassword,string confirmationPassword)
        {
            return actualPassword != confirmationPassword;
        }
    }
}
