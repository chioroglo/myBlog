using MyBlog.Common.Options;

namespace MyBlog.API.Extensions
{
    public static class OptionsInitializer
    {
        public static void InitializeOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptionsWithValidateOnStart<JsonWebTokenOptions>()
                .BindConfiguration(JsonWebTokenOptions.Config)
                .ValidateDataAnnotations();

            services.AddOptionsWithValidateOnStart<CorsPolicyOptions>()
                .BindConfiguration(CorsPolicyOptions.Config)
                .ValidateDataAnnotations();

            services.AddOptionsWithValidateOnStart<PasskeyOptions>()
                .BindConfiguration(PasskeyOptions.Config)
                .ValidateDataAnnotations();

            services.AddOptionsWithValidateOnStart<SemanticAnalysisOptions>()
                .BindConfiguration(SemanticAnalysisOptions.Config)
                .ValidateDataAnnotations();

            services.AddOptionsWithValidateOnStart<PunishmentOptions>()
                .BindConfiguration(PunishmentOptions.Config)
                .ValidateDataAnnotations();

            services.AddOptionsWithValidateOnStart<AzureStorageCredentialOptions>()
                .BindConfiguration(AzureStorageCredentialOptions.Config)
                .ValidateDataAnnotations();

            services.AddOptionsWithValidateOnStart<AvatarOptions>()
                .BindConfiguration(AvatarOptions.Config)
                .ValidateDataAnnotations();

            AddAzureContainerOptions(services, configuration);
        }

        private static void AddAzureContainerOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AvatarContainerOptions>(
                configuration.GetSection("BlobContainers:Avatar"));
        }
    }
}