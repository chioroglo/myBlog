using Microsoft.AspNetCore.Authentication.JwtBearer;
using MyBlog.Common.Dto.Auth;
using MyBlog.Common.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MyBlog.FunctionalTests.Utils;
using MyBlog.Common.Dto.Post;
using MyBlog.FunctionalTests.Utils.Fakers;
using MyBlog.Service.Abstract;

namespace MyBlog.FunctionalTests.E2E;

public class PostShouldBeAsyncProfanityFilteredAfterAddingTest : BaseFunctionalTest
{
    private readonly IPostService _postService;

    public PostShouldBeAsyncProfanityFilteredAfterAddingTest(FunctionalTestWebAppFactory factory) : base(factory)
    {
        _postService = Services.GetRequiredService<IPostService>();
    }

    [Fact]
    public async Task Post_ShouldBe_Async_ProfanityFiltered_AfterAdding()
    {
        // Arrange
        var registrationDto = new RegistrationDtoFaker().Generate();
        registrationDto.ConfirmPassword = registrationDto.Password;
        // Act

        // Register
        var registrationResponse = await TryHitPost<RegistrationDto, UserModel>("api/register", registrationDto);

        // Log In
        var loginResponse = await TryHitPost<PasswordAuthorizeRequest, AuthorizationResponseModel>("api/auth/login",
            new PasswordAuthorizeRequest
            {
                Username = registrationDto.Username,
                Password = registrationDto.Password
            });

        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            JwtBearerDefaults.AuthenticationScheme,
            loginResponse!.AccessToken);

        // Add post
        var postDto = new PostDto
        {
            Title = "mocktitle",
            // Mock post
            Content =
                "Message-driven architectures are becoming increasingly popular in modern .NET applications. However, testing these systems can be challenging—especially when validating that a message was published to RabbitMQ and later processed correctly. In this post, we'll explore how to test message flows using Testcontainers, MassTransit, and ASP.NET Core, ensuring your application behaves as expected from start to finish.",
            Topic = "newtopic"
        };
        var addPostResponse = await TryHitPost<PostDto, PostModel>("api/posts", postDto);

        // Check initial post result
        addPostResponse.Should().NotBeNull();
        addPostResponse.AuthorUsername.Should().Be(registrationDto.Username);
        addPostResponse.AuthorId.Should().Be(registrationResponse!.Id);
        addPostResponse.AuthorInitials.Should().Be($"{registrationDto.FirstName.First()}{registrationDto.LastName.First()}");
        addPostResponse.Title.Should().Be(postDto.Title);
        addPostResponse.Content.Should().Be(postDto.Content);
        addPostResponse.Topic.Should().Be(postDto.Topic);
        addPostResponse.Language.Should().BeNull();

        // Wait until message being processed by MassTransit consumer...
        // with specific amount of retries.
        PostModel? post = null;
        for (var attemptNo = 0; attemptNo < 3; attemptNo++)
        {
            await Task.Delay(500);
            post = await TryHitGet<PostModel>($"api/posts/{addPostResponse.Id}");
            // Language should be set
            if (!string.IsNullOrWhiteSpace(post.Language))
            {
                break;
            }
        }

        post.Should().NotBeNull();
        post.Language.Should().Be("eng");
        post.Id.Should().Be(addPostResponse.Id);
        post.Content.Should().Be(postDto.Content);
    }
}