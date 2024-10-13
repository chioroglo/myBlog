using Domain.Abstract.Messaging;

namespace Domain.Messaging
{
    public record AnalyzePostMessage(int PostId) : IMessage;
}