using Domain.Abstract.Messaging;

namespace Service.Abstract.Messaging;

public interface IMessageBus
{
    Task Publish<TMessage>(TMessage message, TimeSpan? delay = null, CancellationToken ct = default) where TMessage: class, IMessage;
}