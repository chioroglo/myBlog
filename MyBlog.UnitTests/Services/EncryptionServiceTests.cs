using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using MyBlog.Common.Options;
using MyBlog.Common.Validation;
using MyBlog.Service.Auth;

namespace MyBlog.UnitTests.Services;

public class EncryptionServiceTests
{
    private readonly IOptions<JsonWebTokenOptions> _options;
    private readonly EncryptionService _subject;

    public EncryptionServiceTests()
    {
        // Generate an in-memory RSA key pair
        using var rsa = RSA.Create(2048);

        // Export private key in PKCS#8 PEM format
        var privateKey = ExportPrivateKeyPem(rsa);

        // Export public key in PEM format
        var publicKey = ExportPublicKeyPem(rsa);

        _options = new OptionsWrapper<JsonWebTokenOptions>(new()
        {
            PrivateKey = privateKey,
            PublicKey = publicKey,
            Issuer = "issuer",
            Audience = "audience2232",
            AccessTokenValidityTime = TimeSpan.FromMinutes(24)
        });
        _subject = new EncryptionService(_options);
    }


    [Fact]
    public void GenerateNewUniqueRefreshToken_ShouldGenerateToken_Of_RefreshTokenLength()
    {
        // Arrange
        const int expectedLength = EntityConfigurationConstants.RefreshTokenLength;
        
        // Act
        var response = _subject.GenerateRefreshToken();

        // Assert
        response.Value.Length.Should().Be(expectedLength);
    }

    [Theory]
    [InlineData("password", "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8")]
    [InlineData("lovemama123", "9bcd53793c43361386708990a5a7827140deb591910da5fd8649a9b81759ffa6")]
    public void EncryptPassword_ShouldEncrypt_Sha256(string input, string expected)
    {
        // Arrange

        // Act
        var actual = _subject.EncryptPassword(input);

        // Assert
        actual.Should().Be(expected);
    }

    private static string ExportPrivateKeyPem(RSA rsa)
    {
        var pkcs8 = rsa.ExportPkcs8PrivateKey();
        return PemEncode("PRIVATE KEY", pkcs8);
    }

    private static string ExportPublicKeyPem(RSA rsa)
    {
        var publicKey = rsa.ExportSubjectPublicKeyInfo();
        return PemEncode("PUBLIC KEY", publicKey);
    }

    private static string PemEncode(string label, byte[] keyBytes)
    {
        var base64 = Convert.ToBase64String(keyBytes);
        var lines = Enumerable.Range(0, (int)Math.Ceiling(base64.Length / 64.0))
            .Select(i => base64.Substring(i * 64, Math.Min(64, base64.Length - i * 64)));
        return $"-----BEGIN {label}-----\n{string.Join("\n", lines)}\n-----END {label}-----";
    }
}