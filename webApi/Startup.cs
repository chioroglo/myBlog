using API.Extensions;
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

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .LoadConfigurationForJwtBearer(Configuration);

            services.AddCorsWithPolicy(Configuration.GetSection(CorsPolicyOptions.Config).Get<CorsPolicyOptions>());
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddDbContext<BlogDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Blog"))
                , ServiceLifetime.Transient);
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("Redis");
            });

            services.AddAutoMapper(typeof(MappingAssemblyMarker).Assembly);
            services.InitializeRepositories();
            services.InitializeServices();
            services.InitializeOptions(Configuration);
            services.InitializeControllers();
            services.InitializePasskeyFido2CryptoLibrary();
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
            app.UseDatabaseTransactions(); //


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}