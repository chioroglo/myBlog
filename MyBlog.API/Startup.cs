using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.FeatureManagement;
using MyBlog.API.Extensions;
using MyBlog.API.Extensions.Auth;
using MyBlog.API.Middlewares;
using MyBlog.Common;
using MyBlog.Common.Options;
using MyBlog.Data;
using MyBlog.Domain.Abstract;
using MyBlog.Service;
using MyBlog.Service.Abstract;

namespace MyBlog.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .LoadConfigurationForJwtBearer(Configuration);

            services.AddCorsWithPolicy(Configuration);
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddDbContext<BlogDbContext>(
                options =>
                {
                    var connectionString = Configuration.GetConnectionString("Blog");
                    options.UseSqlServer(connectionString);
                    // TODO: Remove after EF core bug will be fixed
                    // https://github.com/dotnet/efcore/issues/35158
                    options.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
                });

            services.AddScoped<IUnitOfWork>(serviceProvider =>
            {
                var context = serviceProvider.GetRequiredService<BlogDbContext>();
                return new UnitOfWork(context);
            });

            services.AddScoped(typeof(IBlobStorageService<>), typeof(AzureBlobStorageService<>));

            services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.AddDelayedMessageScheduler();
                busConfigurator.SetKebabCaseEndpointNameFormatter();
                busConfigurator.AddConsumersFromNamespaceContaining(typeof(MyBlog.API.AssemblyReference));

                var options = MessageBrokerInitializer.AddOptions(Configuration, services);

                switch (options.Provider)
                {
                    case MessageBrokerProvider.InMemory:
                    {
                        MessageBrokerInitializer.ConfigureInMemory(busConfigurator);
                        break;
                    }
                    case MessageBrokerProvider.Rabbit:
                    {
                        var rabbitMqOptions = new RabbitMqOptions();
                        Configuration.GetSection(RabbitMqOptions.Config).Bind(rabbitMqOptions);
                        services.AddOptions<RabbitMqOptions>().Bind(Configuration.GetSection(RabbitMqOptions.Config));
                        MessageBrokerInitializer.ConfigureRabbitMq(busConfigurator, rabbitMqOptions);
                        break;
                    }
                }
            });

            services.AddAutoMapper(config =>
            {
                config.AddMaps(typeof(MappingAssemblyMarker).Assembly);
            });
            services.InitializeOptions(Configuration);
            services.AddCache(Configuration);
            services.InitializeRepositories();
            services.InitializeServices();
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.WriteIndented = true;
            });
            services.InitializePasskeyFido2CryptoLibrary();
            services.AddScoped<BannedUserMiddleware>();
            services.AddScoped<JwtAccessTokenBlacklistMiddleware>();
            services.AddProblemDetails();
            services.AddExceptionHandler<GlobalExceptionHandlingMiddleware>();
            services.AddFeatureManagement();
            services.AddMyHealthChecks(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseExceptionHandler();
            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<BannedUserMiddleware>();
            app.UseMiddleware<JwtAccessTokenBlacklistMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("pulse", new HealthCheckOptions
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                    AllowCachingResponses = false
                });
            });
        }
    }
}