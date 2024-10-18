namespace API.Extensions;

public static class ControllersInitializer
{
    public static void InitializeControllers(this IServiceCollection services) => 
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.WriteIndented = true;
        });
}