namespace API.Extensions
{
    public static class CorsPolicyConfiguration
    {
        public static void AddCorsWithCustomDefaultPolicy(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder
                            .AllowAnyOrigin() // in PRODUCTION
                            .AllowAnyHeader()
                            .WithMethods("GET", "POST", "PUT", "PATCH", "DELETE");
                    });
            });
        }
    }
}