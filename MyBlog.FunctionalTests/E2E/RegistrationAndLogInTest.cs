using MyBlog.Common.Dto.Auth;
using MyBlog.FunctionalTests.Utils;
using FluentAssertions;
using MyBlog.Common.Models;
using System.Net.Http.Json;
using System;
using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MyBlog.Common.Utils;

namespace MyBlog.FunctionalTests.E2E;

public class RegistrationAndLogInTest : BaseFunctionalTest
{
    public RegistrationAndLogInTest(FunctionalTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task ShouldRegister_AndLogIn_LogOut_Successfully()
    {
        // Arrange
        const string username = "CoolUsername";
        const string password = "P@ssword!";

        // Act


            // Register
        var registrationResponse = await TryHitPost<RegistrationDto, UserModel>("api/register", new()
        {
            Username = username,
            Password = password,
            ConfirmPassword = password
        });

            // Log In
        var loginResponseMessage = await HttpClient.PostAsJsonAsync("api/auth/login", new PasswordAuthorizeRequest
        {
            Password = password,
            Username = username
        });
        var loginResponse = await loginResponseMessage.Content.ReadFromJsonAsync<AuthorizationResponseModel>();
        loginResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);

            // Log Out
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            JwtBearerDefaults.AuthenticationScheme,
            loginResponse.AccessToken);
        var logoutResponseMessage = await HttpClient.PostAsJsonAsync("api/auth/logout", string.Empty);
        logoutResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);

        // Assert
        registrationResponse.Should().NotBeNull();
        loginResponse.Should().NotBeNull();
        loginResponse.AccessToken.Should().NotBeEmpty();
        loginResponse.UserId.Should().Be(registrationResponse.Id);

        loginResponseMessage.Headers.TryGetValues("Set-Cookie", out var setCookieHeaderValues);
        setCookieHeaderValues.Should().NotBeNull();
        setCookieHeaderValues.Should().HaveCount(1);
            // Assert that refresh-token cookie was set
        setCookieHeaderValues.Should().Contain(headerValue => headerValue.StartsWith(JwtUtils.CookieRefreshTokenKey));

            // Assert that refresh-token cookie was cleaned, which means it was set as empty string
        logoutResponseMessage.Headers.TryGetValues("Set-Cookie", out setCookieHeaderValues);
        setCookieHeaderValues.Should().NotBeNull();
        setCookieHeaderValues.Should().HaveCount(1);
        setCookieHeaderValues.Should()
            .Contain(headerValue => headerValue.StartsWith($"{JwtUtils.CookieRefreshTokenKey}=;"));
    }
}