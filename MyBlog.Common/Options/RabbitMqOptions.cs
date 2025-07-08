namespace MyBlog.Common.Options;

public class RabbitMqOptions
{
    public const string Config = "MessageBus";
    public string Host { get; set; }
    public string VirtualHost { get; set; }
    public ushort Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}