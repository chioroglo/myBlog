using DAL.Configurations;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public partial class BlogDbContext : DbContext
    {
        public BlogDbContext()
        {

        }

        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }


        public DbSet<UserEntity>? Users { get; set; }

        public DbSet<PostEntity>? Posts { get; set; }

        public DbSet<CommentEntity>? Comments { get; set; }

        public DbSet<AvatarEntity>? Avatar { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var assembly = typeof(ConfigurationsAssemblyMarker).Assembly;

            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        }
    }
}
