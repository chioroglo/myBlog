using DAL.Repositories.Abstract.Base;

namespace API.Extensions;

public static class RepositoriesInitializer
{
    public static void InitializeRepositories(this IServiceCollection services)
    {
        services.Scan(scan =>
            scan.FromAssemblies(typeof(DAL.AssemblyReference).Assembly)
                .AddClasses(classes => classes.AssignableTo(typeof(BaseRepository<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());
    }
}