namespace API.Extensions
{
    public static class CorsPolicyConfiguration
    {

        private const string DevelopmentAppUrl = "http://localhost:3000";

        public static void AddCorsWithCustomDefaultPolicy(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder
                        .WithOrigins(DevelopmentAppUrl)
                        //.AllowAnyOrigin() in PRODUCTION
                        .AllowAnyHeader()
                        .WithMethods("GET", "POST", "PUT", "PATCH", "DELETE");
                    });
            });
        }
    }
}
