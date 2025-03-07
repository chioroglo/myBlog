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

        // Log In
        var loginResponseMessage = await HttpClient.PostAsJsonAsync("api/auth/login", new PasswordAuthorizeRequest
        {
            Password = Password,
            Username = Username
        }, _ct);
        loginResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        var loginResponse = await loginResponseMessage.Content.ReadFromJsonAsync<AuthorizationResponseModel>(_ct);
        var oldAccessToken = loginResponse.AccessToken;

        // Change Password
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            JwtBearerDefaults.AuthenticationScheme,
            oldAccessToken);

        var changePasswordMessage = await HttpClient.PostAsJsonAsync("api/auth/password/change", new ChangePasswordDto
        {
            CurrentPassword = Password,
            NewPassword = NewPassword,
            ConfirmNewPassword = NewPassword,
            UserId = user.Id
        }, _ct);
        var changePasswordResponse = await  
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