using Testcontainers.MsSql;

namespace MyBlog.FunctionalTests.Utils;

public static class ContainersSetup
{
    public static MsSqlContainer BuildMsSqlContainer() => new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithPassword("P@ssword!")
        .Build();
}