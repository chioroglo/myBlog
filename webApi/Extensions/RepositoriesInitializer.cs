using DAL.Repositories;
using DAL.Repositories.Abstract;

namespace API.Extensions
{
    public static class RepositoriesInitializer
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IAvatarRepository, AvatarRepository>();
            services.AddScoped<IPostReactionRepository, PostReactionRepository>();
            services.AddScoped<ITopicRepository, TopicRepository>();
        }
    }
}