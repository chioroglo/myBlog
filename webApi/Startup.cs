﻿using API.Extensions;
using API.Extensions.Auth;
using Common;
using Common.Options;
using DAL;
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

            var authenticationBuilder =
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
            authenticationBuilder.LoadConfigurationForJwtBearer(Configuration);

            services.AddCorsWithPolicy(
                Configuration.GetSection(CorsPolicyOptions.Config).Get<CorsPolicyOptions>()
                );
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            var connectionString = Configuration.GetConnectionString("Blog");
            services.AddDbContext<BlogDbContext>(options =>
                {
                    options.UseSqlServer(connectionString);
                },
                ServiceLifetime.Transient);

            services.AddAutoMapper(typeof(MappingAssemblyMarker).Assembly);
            services.InitializeRepositories();
            services.InitializeServices();
            services.InitializeOptions(Configuration);
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
                    context.Context.Response.Headers.Add("Cache-Control", "no-cache, no-store");
                    context.Context.Response.Headers.Add("Expires", "-1");
                }
            });

            app.UseExceptionHandling(); //
            app.UseRouting();
            app.UseCors();


            app.UseAuthentication();
            app.UseAuthorization();
            app.UseDatabaseTransactions(); //


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}