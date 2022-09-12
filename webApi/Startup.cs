using API.Extensions;
using API.Extensions.Auth;
using DAL;
using Mapping;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

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
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();


            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<BlogDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddAutoMapper(typeof(MappingAssemblyMarker).Assembly);
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
            app.UseExceptionHandling();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.Use(async (context, next) =>
            {
                await next();
            });

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseDatabaseTransactions();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
               // endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/");
            });
        }
    }
}
