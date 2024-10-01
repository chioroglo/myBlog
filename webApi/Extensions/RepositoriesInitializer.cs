using DAL.Repositories;
using DAL.Repositories.Abstract;

namespace API.Extensions
{
    public static class RepositoriesInitializer
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IPostRepository, PostRepository>();
            services.AddTransient<ICommentRepository, CommentRepository>();
            services.AddTransient<IPostReactionRepository, PostReactionRepository>();
            services.AddTransient<IAvatarRepository, AvatarRepository>();
            services.AddTransient<IPasskeyRepository, EfPasskeyRepository>();
        }
    }
}