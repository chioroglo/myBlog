using MassTransit;
using MyBlog.Common.Options;

namespace MyBlog.API.Extensions;

public static class MessageBrokerInitializer
{
    public static MessageBrokerOptions AddOptions(IConfiguration configuration, IServiceCollection services)
    {
        var options = new MessageBrokerOptions();
        configuration.GetSection(MessageBrokerOptions.Config).Bind(options);
        services.AddOptions<MessageBrokerOptions>().Bind(
            configuration.GetSection(MessageBrokerOptions.Config)
        );
        return options;
    }

    public static void ConfigureInMemory(IBusRegistrationConfigurator busConfigurator)
    {
        busConfigurator.UsingInMemory((context, configurator) =>
        {
            configurator.UseDelayedMessageScheduler();
            configurator.MapProducers(context)
                .MapConsumers(context);
            configurator.ConfigureEndpoints(context);
        });
    }

    public static void ConfigureRabbitMq(IBusRegistrationConfigurator busConfigurator, RabbitMqOptions options)
    {
        busConfigurator.UsingRabbitMq((context, configurator) =>
        {
            configurator.Host(options.Host, options.Port, options.VirtualHost, h =>
            {
                h.Username(options.Username);
                h.Password(options.Password);
            });

            configurator.UseDelayedMessageScheduler();
            configurator.MapProducers(context)
                .MapConsumers(context);
            configurator.ConfigureEndpoints(context);
        });
    }
}