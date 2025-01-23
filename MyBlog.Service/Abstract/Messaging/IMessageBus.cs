using MyBlog.Domain.Abstract.Messaging;

namespace MyBlog.Service.Abstract.Messaging;

public interface IMessageBus
{
    Task Publish<TMessage>(TMessage message, TimeSpan? delay = null, CancellationToken ct = default) where TMessage: class, IMessage;
}