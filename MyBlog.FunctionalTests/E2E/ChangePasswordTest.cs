using Microsoft.Extensions.DependencyInjection;
using MyBlog.Common.Dto.Auth;
using MyBlog.Common.Models;
using MyBlog.Domain;
using MyBlog.Domain.Abstract;
using MyBlog.FunctionalTests.Utils;
using MyBlog.Service.Abstract;
using MyBlog.Service.Abstract.Auth;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Json;
using System.Net;
using System.Net.Http.Headers;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MyBlog.Common.Dto.Post;

namespace MyBlog.FunctionalTests.E2E;

public class ChangePasswordTest : BaseFunctionalTest
{
    private readonly IUserService _userService;
    private readonly IRegistrationService _registrationService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CancellationToken _ct = CancellationToken.None;
    private const string Username = "TestName1";
    private const string Password = "P@ssword!";
    private const string NewPassword = "NeewP@ssword";

    public ChangePasswordTest(FunctionalTestWebAppFactory factory) : base(factory)
    {
        _userService = Services.GetRequiredService<IUserService>();
        _unitOfWork = Services.GetRequiredService<IUnitOfWork>();
        _registrationService = Services.GetRequiredService<IRegistrationService>();
    }

    [Fact]
    public async Task ShouldChangePassword_And_UnableToLogIn_WithOldPassword()
    {
        // Arrange
        var user = await CreateMockUser();

        // Act
            // Log In
        var loginResponse = await TryAuthorize(Username, Password, expectedStatusCode: HttpStatusCode.OK);
        var oldAccessToken = loginResponse.AccessToken;

            // Change Password
        SetAuthorizationHeader(oldAccessToken);

        var changePasswordMessage = await HttpClient.PatchAsJsonAsync("api/auth/password/change", new ChangePasswordDto
        {
            CurrentPassword = Password,
            NewPassword = NewPassword,
            ConfirmNewPassword = NewPassword,
            UserId = user.Id
        }, _ct);
        changePasswordMessage.StatusCode.Should().Be(HttpStatusCode.OK);

            // Try to reach API with old token and create post (should fail)
        var createPostMessage = await HttpClient.PostAsJsonAsync("api/posts", new PostDto
        {
            Title = "Test",
            Content = "Test"
        }, _ct);
        createPostMessage.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            // Try to log In with old password
        HttpClient.DefaultRequestHeaders.Clear();
        await TryAuthorize(Username, Password, expectedStatusCode: HttpStatusCode.BadRequest);
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
        }, _ct);

        loginResponseMessage.StatusCode.Should().Be(expectedStatusCode);
        return await loginResponseMessage.Content.ReadFromJsonAsync<AuthorizationResponseModel>(_ct);
    }
    private void SetAuthorizationHeader(string value)
    {
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            JwtBearerDefaults.AuthenticationScheme,
            value);
    }
    private async Task<User> CreateMockUser()
    {
        var user = await _registrationService.RegisterAsync(new RegistrationDto
        {
            Username = Username,
            Password = Password,
            ConfirmPassword = Password
        }, _ct);
        return user;
    }
}