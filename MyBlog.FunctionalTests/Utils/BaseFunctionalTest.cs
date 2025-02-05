using System.Net.Http.Json;
using System.Net;
using FluentAssertions;

namespace MyBlog.FunctionalTests.Utils;

public class BaseFunctionalTest : IClassFixture<FunctionalTestWebAppFactory>
{
    protected HttpClient HttpClient { get; init; }
    public BaseFunctionalTest(FunctionalTestWebAppFactory factory)
    {
        HttpClient = factory.CreateClient();
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
}