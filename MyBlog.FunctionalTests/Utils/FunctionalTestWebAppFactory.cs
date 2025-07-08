using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MyBlog.API;
using MyBlog.API.Extensions;
using MyBlog.Common.Options;
using MyBlog.Data;
using Testcontainers.MsSql;
using Testcontainers.RabbitMq;

namespace MyBlog.FunctionalTests.Utils;

public class FunctionalTestWebAppFactory : WebApplicationFactory<Program> , IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer = ContainersSetup.BuildMsSqlContainer();
    private readonly RabbitMqContainer _rabbitMqContainer = ContainersSetup.BuildRabbitMqContainer();
    private IConfiguration _configuration;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");

        builder.ConfigureAppConfiguration((context, config) =>
        {
            var appsettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Test.json");

            config.AddEnvironmentVariables();
            config.AddJsonFile(appsettingsPath, optional: false, reloadOnChange: false);
        });

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<BlogDbContext>));

            services.AddDbContext<BlogDbContext>(options =>
            {
                options.UseSqlServer(_dbContainer.GetConnectionString());
            });
        });
    }

    public virtual async Task InitializeAsync()
    {
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
    }
}