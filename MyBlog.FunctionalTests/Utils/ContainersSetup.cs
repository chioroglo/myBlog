using Testcontainers.MsSql;
using Testcontainers.RabbitMq;
using Testcontainers.Redis;

namespace MyBlog.FunctionalTests.Utils;

public static class ContainersSetup
{
    public static MsSqlContainer BuildMsSqlContainer() => new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithPortBinding(1433, true)
        .WithPassword("P@ssword!")
        .Build();

    public static RedisContainer BuildRedisContainer() => new RedisBuilder()
        .WithPortBinding(6379, true)
        .WithImage("redis:7.2.3")
        .Build();

    public static class RabbitSetup
    {
        public const string Vhost = "/";
        public const string Username = "admin@admin.com";
        public const string Password = "admin123";
    }

    public static RabbitMqContainer BuildRabbitMqContainer() => new RabbitMqBuilder()
        .WithImage("masstransit/rabbitmq")
        .WithPortBinding(5672, true)     // AMQP port
        .WithPortBinding(15672, true)    // Management UI port
        .WithEnvironment("RABBITMQ_DEFAULT_USER", RabbitSetup.Username)
        .WithEnvironment("RABBITMQ_DEFAULT_PASS", RabbitSetup.Password)
        .WithCleanUp(true)
        .Build();
}