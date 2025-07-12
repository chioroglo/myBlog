using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MyBlog.API;
using MyBlog.FunctionalTests.Utils;
using MyBlog.Service;
using MyBlog.Service.Abstract;
using Testcontainers.Redis;

namespace MyBlog.FunctionalTests.Integration.Cache;

public sealed class CacheTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly RedisContainer _redisContainer = ContainersSetup.BuildRedisContainer();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");

        builder.ConfigureAppConfiguration((context, config) =>
        {
            var appsettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Test.json");

            config.AddEnvironmentVariables();
            config.AddJsonFile(appsettingsPath, optional: false, reloadOnChange: false);
        });


        builder.ConfigureTestServices(services =>
        {
            services.AddScoped<RedisDistributedCacheService>();
            services.AddScoped<InMemoryCacheService>();
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = _redisContainer.GetConnectionString();
            });
        });
    }
    public async Task InitializeAsync()
    {
        await _redisContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await base.DisposeAsync();
        await _redisContainer.StopAsync();
    }
}