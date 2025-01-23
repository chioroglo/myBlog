using MassTransit;
using MyBlog.API.EventConsumers;
using MyBlog.Domain.Messaging;

namespace MyBlog.API.Extensions;

public static class MessageBusInitializer
{

    public static IBusFactoryConfigurator MapProducers(this IBusFactoryConfigurator configurator,
        IBusRegistrationContext ctx)
    {
        configurator.Send<AnalyzePostMessage>(x => x.UseRoutingKeyFormatter(_ => RoutingKeys.AnalyzePost));
        configurator.Message<AnalyzePostMessage>(x => x.SetEntityName(nameof(AnalyzePostMessage)));
        return configurator;
    }

    public static IBusFactoryConfigurator MapConsumers(this IBusFactoryConfigurator configurator,
        IBusRegistrationContext ctx)
    {
        configurator.ReceiveEndpoint(RoutingKeys.AnalyzePost, endpointConfigurator =>
        {
            endpointConfigurator.ConfigureConsumer<AnalyzePostMessageConsumer>(ctx);
        });

        return configurator;
    }
}

internal static class RoutingKeys
{
    internal const string AnalyzePost = "analyze-post";
}