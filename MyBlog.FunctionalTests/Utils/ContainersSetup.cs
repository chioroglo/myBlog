using Testcontainers.MsSql;
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
}