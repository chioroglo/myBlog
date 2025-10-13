using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Networks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MyBlog.API;
using MyBlog.Data;
using MyBlog.Service;
using MyBlog.Service.Abstract;
using Testcontainers.MsSql;
using Testcontainers.RabbitMq;
using Testcontainers.Redis;

namespace MyBlog.FunctionalTests.Utils;

public class FunctionalTestWebAppFactory : WebApplicationFactory<Program> , IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer;
    private readonly RabbitMqContainer _rabbitMqContainer;
    private readonly RedisContainer _redisContainer;
    private readonly INetwork _network;
    private IConfiguration _configuration;

    public FunctionalTestWebAppFactory()
    {
        _network = new NetworkBuilder()
            .WithName($"sut-{Guid.NewGuid()}")
            .Build();

        _dbContainer = ContainersSetup.BuildMsSqlContainer(_network);
        _redisContainer = ContainersSetup.BuildRedisContainer(_network);
        _rabbitMqContainer = ContainersSetup.BuildRabbitMqContainer(_network);
    }

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