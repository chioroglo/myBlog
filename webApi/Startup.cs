﻿using API.Extensions;
using API.Extensions.Auth;
using API.Middlewares;
using Common;
using DAL;
using Domain.Abstract;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

namespace API
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
                });

            services.AddScoped<IUnitOfWork>(serviceProvider =>
            {
                var context = serviceProvider.GetRequiredService<BlogDbContext>();
                return new UnitOfWork(context);
            });

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("Redis");
            });

            services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.AddDelayedMessageScheduler();
                busConfigurator.SetKebabCaseEndpointNameFormatter();
                busConfigurator.AddConsumersFromNamespaceContaining(typeof(API.AssemblyReference));

                busConfigurator.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(Configuration["MessageBus:Host"]!, h =>
                    {
                        h.Username(Configuration["MessageBus:Username"]!);
                        h.Password(Configuration["MessageBus:Password"]!);
                    });
                    configurator.UseDelayedMessageScheduler();
                    configurator.MapProducers(context)
                        .MapConsumers(context);
                    configurator.ConfigureEndpoints(context);
                });
            });

            services.AddAutoMapper(typeof(MappingAssemblyMarker).Assembly);
            services.InitializeOptions(Configuration);
            services.InitializeRepositories();
            services.InitializeServices();
            services.InitializeControllers();
            services.InitializePasskeyFido2CryptoLibrary();
            services.AddScoped<BannedUserMiddleware>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = (context) =>
                {
                    context.Context.Response.Headers.Append("Cache-Control", "no-cache, no-store");
                    context.Context.Response.Headers.Append("Expires", "-1");
                }
            });

            app.UseExceptionHandling(); //
            app.UseRouting();
            app.UseCors();


            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<BannedUserMiddleware>();
            //app.UseDatabaseTransactions(); TODO: Bring back after DB transaction will be fixed


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}