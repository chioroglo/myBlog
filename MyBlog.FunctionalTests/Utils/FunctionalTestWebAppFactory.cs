using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MyBlog.API;
using MyBlog.API.Extensions;
using MyBlog.Common.Options;
using MyBlog.Data;
using MyBlog.Service;
using MyBlog.Service.Abstract;
using Testcontainers.MsSql;
using Testcontainers.RabbitMq;
using Testcontainers.Redis;

namespace MyBlog.FunctionalTests.Utils;

public class FunctionalTestWebAppFactory : WebApplicationFactory<Program> , IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer = ContainersSetup.BuildMsSqlContainer();
    private readonly RabbitMqContainer _rabbitMqContainer = ContainersSetup.BuildRabbitMqContainer();
    private readonly RedisContainer _redisContainer = ContainersSetup.BuildRedisContainer();

    private IConfiguration _configuration;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");

        builder.ConfigureAppConfiguration((context, config) =>
        {
            var appsettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Test.json");

            config.AddEnvironmentVariables();
            config.AddJsonFile(appsettingsPath, optional: false, reloadOnChange: false);

            // Override Mass Transit Rabbit MQ integration
            config.AddInMemoryCollection(new Dictionary<string, string>
            {
                ["MessageBus:Host"] = _rabbitMqContainer.Hostname,
                ["MessageBus:VirtualHost"] = ContainersSetup.RabbitSetup.Vhost,
                ["MessageBus:Port"] = _rabbitMqContainer.GetMappedPublicPort(5672).ToString(),
                ["MessageBus:Username"] = ContainersSetup.RabbitSetup.Username,
                ["MessageBus:Password"] = ContainersSetup.RabbitSetup.Password
            }!);
        });


        builder.ConfigureTestServices(services =>
        {

            #region Override MS SQL integration

            services.RemoveAll(typeof(DbContextOptions<BlogDbContext>));

            services.AddDbContext<BlogDbContext>(options =>
            {
                options.UseSqlServer(_dbContainer.GetConnectionString());
            });

            #endregion

            #region Override Redis Integration

            services.RemoveAll<ICacheService>();
            services.AddScoped<ICacheService, RedisDistributedCacheService>();

            services.AddStackExchangeRedisCache(options =>
            {
                var connStr = _redisContainer.GetConnectionString();
                options.Configuration = connStr;
            });

            #endregion
        });
    }

    public virtual async Task InitializeAsync()
    {
        await _redisContainer.StartAsync();
        await _rabbitMqContainer.StartAsync();
        await _dbContainer.StartAsync();

        await using (var scope = Services.CreateAsyncScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogDbContext>();

            await dbContext.Database.EnsureCreatedAsync();
            await dbContext.DisposeAsync();
        }
    }

    public new virtual async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _rabbitMqContainer.StopAsync();
        await _redisContainer.StopAsync();
    }
}