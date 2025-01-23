using MyBlog.Data.Repositories.Abstract.Base;

namespace MyBlog.API.Extensions;

public static class RepositoriesInitializer
{
    public static void InitializeRepositories(this IServiceCollection services)
    {
        services.Scan(scan =>
            scan.FromAssemblies(typeof(Data.AssemblyReference).Assembly)
                .AddClasses(classes => classes.AssignableTo(typeof(BaseRepository<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());
    }
}