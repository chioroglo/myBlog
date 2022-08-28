using DAL;
using Mapping;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace webApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            AuthenticationBuilder authenticationBuilder = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
            
            authenticationBuilder.LoadConfigurationForJwtBearer(Configuration);
            services.AddControllers();
            services.AddAutoMapper(
                typeof(ApplicationAssemblyMarker).Assembly
                );

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen();

            string connection = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<BlogDbContext>(options =>
            {
                options.UseSqlServer(connection);

            });

            services.InitializeRepositories();
            services.InitializeServices();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.Use(async (context, next) =>
            {
                var endpoint = context.GetEndpoint();
                var rulesEndpoint = (endpoint as RouteEndpoint)?.RoutePattern.RawText;
                Console.WriteLine();
                Console.WriteLine(endpoint?.DisplayName);
                Console.WriteLine(rulesEndpoint);
                await next();
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
               // endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/");
            });
        }
    }
}
