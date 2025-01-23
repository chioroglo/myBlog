using MyBlog.Domain.Abstract.Messaging;

namespace MyBlog.Domain.Messaging
{
    public record AnalyzePostMessage(int PostId) : IMessage;
}