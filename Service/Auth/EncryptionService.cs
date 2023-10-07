using Common.Dto.Auth;
using Microsoft.IdentityModel.Tokens;
using Service.Abstract.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Common;
using Common.Options;
using Microsoft.Extensions.Options;

namespace Service.Auth
{
    public class EncryptionService : IEncryptionService
    {
        private readonly JsonWebTokenOptions _jwtOptions;

        public EncryptionService(IOptions<JsonWebTokenOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }


        public string GenerateAccessToken(AuthenticateResponse userData)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _jwtOptions.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = new Claim[]
            {
                new Claim(TokenClaimNames.Id, userData.Id.ToString()),
                new Claim(TokenClaimNames.Username, userData.Username)
            };

            var token = new JwtSecurityToken(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ValidityTimeMinutes),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string EncryptPassword(string phrase)
        {
            var byteArrayPhrase = Encoding.UTF8.GetBytes(phrase);
            var algorithm = SHA256.Create();
            var hashBytes = algorithm.ComputeHash(byteArrayPhrase);
            var hash = BitConverter.ToString(hashBytes).ToLower().Replace("-","");
            return hash;
        }
    }
}