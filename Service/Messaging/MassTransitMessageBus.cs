using Domain.Abstract.Messaging;
using MassTransit;
using Service.Abstract.Messaging;

namespace Service.Messaging;

public class MassTransitMessageBus : IMessageBus
{
    private readonly IPublishEndpoint _bus;

    public MassTransitMessageBus(IPublishEndpoint bus)
    {
        _bus = bus;
    }

    public async Task Publish<TMessage>(TMessage message, TimeSpan? delay = null, CancellationToken ct = default)
        where TMessage : class, IMessage
    {
        delay ??= TimeSpan.Zero;
        await _bus.Publish(message, context => { context.Delay = delay; }, ct);
    }
}