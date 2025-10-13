using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyBlog.Common;
using MyBlog.Common.Dto.Auth;
using MyBlog.Common.Options;
using MyBlog.Common.Validation;
using MyBlog.Service.Abstract.Auth;

namespace MyBlog.Service.Auth
{
    public class EncryptionService : IEncryptionService
    {
        private readonly JsonWebTokenOptions _jwtOptions;

        public EncryptionService(IOptions<JsonWebTokenOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public string GenerateAccessToken(int userId, string username)
        {
            var rsa = RSA.Create();
            rsa.ImportFromPem(_jwtOptions.PrivateKey);
            var securityKey = new RsaSecurityKey(rsa);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256);

            var notBefore = DateTime.UtcNow;
            var expires = notBefore.Add(_jwtOptions.AccessTokenValidityTime);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
                Claims = new Dictionary<string, object>
                {
                    { TokenClaimNames.Id, userId.ToString(CultureInfo.InvariantCulture) },
                    { TokenClaimNames.Username, username }
                },
                SigningCredentials = credentials,
                NotBefore = notBefore,
                Expires = expires,
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);
            var jwt = handler.WriteToken(token);
            return jwt;
        }

        public string EncryptPassword(string phrase)
        {
            var byteArrayPhrase = Encoding.UTF8.GetBytes(phrase);
            using var algorithm = SHA256.Create();
            var hashBytes = algorithm.ComputeHash(byteArrayPhrase);
            var hash = BitConverter.ToString(hashBytes).ToLower().Replace("-", "");
            return hash;
        }

        public RefreshToken GenerateRefreshToken()
        {
            const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            const int tokenLength = EntityConfigurationConstants.RefreshTokenLength;

            using var rng = RandomNumberGenerator.Create();
            var sb = new StringBuilder(tokenLength);
            var randomNumber = new byte[4];
            for (var i = 0; i < tokenLength; i++)
            {
                rng.GetBytes(randomNumber);
                var randomIndexInAlphabet = BitConverter.ToInt32(randomNumber, 0);
                if (randomIndexInAlphabet < 0)
                {
                    randomIndexInAlphabet *= -1;
                }

                randomIndexInAlphabet %= alphabet.Length;

                sb.Append(alphabet[randomIndexInAlphabet]);
            }

            return new RefreshToken
            {
                Value = sb.ToString(),
                ExpiresAt = DateTime.UtcNow.Add(_jwtOptions.RefreshTokenValidityTime)
            };
        }
    }
}