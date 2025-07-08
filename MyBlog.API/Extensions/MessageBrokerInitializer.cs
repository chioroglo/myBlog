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

    public static void ConfigureRabbitMq(IBusRegistrationConfigurator busConfigurator, IConfiguration configuration)
    {
        var host = configuration["MessageBus:Host"]!;
        var username = configuration["MessageBus:Username"]!;
        var password = configuration["MessageBus:Password"]!;
        var port = ushort.Parse(configuration["MessageBus:Port"]!);
        var virtualHost = configuration["MessageBus:VirtualHost"]!;

        busConfigurator.UsingRabbitMq((context, configurator) =>
        {
            configurator.Host(host,port,virtualHost,h =>
            {
                h.Username(username);
                h.Password(password);
            });
            configurator.UseDelayedMessageScheduler();
            configurator.MapProducers(context)
                .MapConsumers(context);
            configurator.ConfigureEndpoints(context);
        });
    }
}