using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MyBlog.API;
using MyBlog.Data;
using Testcontainers.MsSql;

namespace MyBlog.FunctionalTests.Utils;

public class FunctionalTestWebAppFactory : WebApplicationFactory<Program> , IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer = ContainersSetup.BuildMsSqlContainer();

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

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        await using (var scope = Services.CreateAsyncScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<BlogDbContext>();

            await dbContext.Database.EnsureCreatedAsync();
            await dbContext.DisposeAsync();
        }
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }
}