using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using MyBlog.Data;
using MyBlog.FunctionalTests.Utils;
using MyBlog.Service;
using MyBlog.Service.Abstract;
using Testcontainers.Redis;

namespace MyBlog.FunctionalTests.Integration.Cache;

public sealed class CacheTestWebAppFactory : FunctionalTestWebAppFactory
{
    private readonly RedisContainer _redisContainer = ContainersSetup.BuildRedisContainer();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

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

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        await _redisContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await base.DisposeAsync();
        await _redisContainer.StopAsync();
    }
}