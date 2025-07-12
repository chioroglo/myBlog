using Microsoft.Extensions.DependencyInjection;
using MyBlog.Common.Dto.Auth;
using MyBlog.Common.Models;
using MyBlog.Domain;
using MyBlog.Domain.Abstract;
using MyBlog.FunctionalTests.Utils;
using MyBlog.Service.Abstract;
using MyBlog.Service.Abstract.Auth;
using System.Net.Http.Json;
using System.Net;
using System.Net.Http.Headers;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MyBlog.Common.Dto.Post;
using MyBlog.FunctionalTests.Utils.Fakers;

namespace MyBlog.FunctionalTests.E2E;

public class ChangePasswordTest : BaseFunctionalTest
{
    private readonly IRegistrationService _registrationService;

    public ChangePasswordTest(FunctionalTestWebAppFactory factory) : base(factory)
    {
        _registrationService = Services.GetRequiredService<IRegistrationService>();
    }

    [Fact]
    public async Task ShouldChangePassword_And_UnableToLogIn_WithOldPassword()
    {
        // Arrange
        var (user, dto) = await CreateMockUser();

        // Act
            // Log In
        var loginResponse = await TryAuthorize(user.Username, dto.Password, expectedStatusCode: HttpStatusCode.OK);
        var oldAccessToken = loginResponse.AccessToken;

            // Change Password
        SetAuthorizationHeader(oldAccessToken);

        var newPasswordDto = new RegistrationDtoFaker().Generate();
        newPasswordDto.ConfirmPassword = newPasswordDto.Password;
        var changePasswordMessage = await HttpClient.PatchAsJsonAsync("api/auth/password/change", new ChangePasswordDto
        {
            CurrentPassword = dto.Password,
            NewPassword = newPasswordDto.Password,
            ConfirmNewPassword = newPasswordDto.ConfirmPassword,
            UserId = user.Id
        });
        changePasswordMessage.StatusCode.Should().Be(HttpStatusCode.OK);

            // Try to reach API with old token and create post (should fail)
        var createPostMessage = await HttpClient.PostAsJsonAsync("api/posts", new PostDto
        {
            Title = "Test",
            Content = "Test"
        });
        createPostMessage.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            // Try to log In with old password
        HttpClient.DefaultRequestHeaders.Clear();
        var authAttemptResponse = await TryAuthorize(user.Username, dto.Password , expectedStatusCode: HttpStatusCode.BadRequest);
        authAttemptResponse.AccessToken.Should().BeNull();
    }

    private async Task<AuthorizationResponseModel?> TryAuthorize(
        string username,
        string password,
        HttpStatusCode expectedStatusCode)
    {
        var loginResponseMessage = await HttpClient.PostAsJsonAsync("api/auth/login", new PasswordAuthorizeRequest
        {
            Password = password,
            Username = username
        });

        loginResponseMessage.StatusCode.Should().Be(expectedStatusCode);
        return await loginResponseMessage.Content.ReadFromJsonAsync<AuthorizationResponseModel>();
    }
    private void SetAuthorizationHeader(string value)
    {
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            JwtBearerDefaults.AuthenticationScheme,
            value);
    }
    private async Task<(User,RegistrationDto)> CreateMockUser()
    {
        var dto = new RegistrationDtoFaker().Generate();
        dto.ConfirmPassword = dto.Password;
        var user = await _registrationService.RegisterAsync(dto, CancellationToken.None);
        return (user,dto);
    }
}