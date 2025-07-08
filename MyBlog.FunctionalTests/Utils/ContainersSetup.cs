using DotNet.Testcontainers.Builders;
using Testcontainers.MsSql;
using Testcontainers.RabbitMq;
using Testcontainers.Redis;

namespace MyBlog.FunctionalTests.Utils;

public static class ContainersSetup
{
    public static MsSqlContainer BuildMsSqlContainer() => new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithPassword("P@ssword!")
        .Build();

    public static RedisContainer BuildRedisContainer() => new RedisBuilder()
        .WithImage("redis:7.2.3")
        .Build();

    public static RabbitMqContainer BuildRabbitMqContainer() => new RabbitMqBuilder()
        .WithImage("masstransit/rabbitmq")
        .WithName("blog-mq")
        .WithHostname("blog-mq")
        .WithPortBinding(5672)     // AMQP port
        .WithPortBinding(15672)    // Management UI port
        .WithEnvironment("RABBITMQ_DEFAULT_USER", "admin@admin.com")
        .WithEnvironment("RABBITMQ_DEFAULT_PASS", "admin123")
        .WithCleanUp(true)
        .Build();

}