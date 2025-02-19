using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using MyBlog.Domain;

namespace MyBlog.Data
{
    public partial class BlogDbContext : DbContext
    {
        public BlogDbContext()
        {
        }

        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Avatar> Avatars { get; set; }
        public DbSet<PostReaction> PostReactions { get; set; }
        public DbSet<Passkey> Passkeys { get; init; }
        public DbSet<UserWarning> UserWarnings { get; init; }
        public DbSet<UserBanLog> UserBans { get; init; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer();
            // TODO: Remove after EF core bug will be fixed
            // https://github.com/dotnet/efcore/issues/35158
            optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var assembly = typeof(MyBlog.Data.AssemblyReference).Assembly;

            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        }
    }
}