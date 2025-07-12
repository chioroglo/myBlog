using System.Net.Http.Json;
using System.Net;
using FluentAssertions;

namespace MyBlog.FunctionalTests.Utils;

public class BaseFunctionalTest : IClassFixture<FunctionalTestWebAppFactory>
{
    protected HttpClient HttpClient { get; init; }
    protected IServiceProvider Services { get; init; }

    public BaseFunctionalTest(FunctionalTestWebAppFactory factory)
    {
        HttpClient = factory.CreateClient();
        Services = factory.Services;
    }

    protected async Task<TResponse?> TryHitPost<TRequest, TResponse>(
        string url,
        TRequest payload,
        HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
            where TRequest : class, new()
            where TResponse : class, new()
    {
        var response = await HttpClient.PostAsJsonAsync(url, payload);
        var jsonResponse = await response.Content.ReadFromJsonAsync<TResponse>();
        response.StatusCode.Should().Be(expectedStatusCode);
        return jsonResponse;
    }

    protected async Task<TResponse?> TryHitGet<TResponse>(string url,
        HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
    {
        var response = await HttpClient.GetAsync(url);
        var jsonResponse = await response.Content.ReadFromJsonAsync<TResponse>();
        response.StatusCode.Should().Be(expectedStatusCode);
        return jsonResponse;
    }
}