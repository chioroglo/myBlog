using DAL.Configurations.Abstract;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static DAL.Configurations.EntityConfigurationConstants;

namespace DAL.Configurations
{
    public class PostEntityConfiguration : BaseEntityConfiguration<Post>
    {
        public override void ConfigureNonPkProperties(EntityTypeBuilder<Post> builder)
        {

            builder.ToTable(nameof(Post));

            builder.HasMany(e => e.Comments)
                .WithOne(e => e.Post)
                .HasForeignKey(e => e.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.User)
                .WithMany(e => e.Posts)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(e => e.Content)
                .HasMaxLength(POST_MAX_LENGTH)
                .IsRequired();

            builder.Property(e => e.Title)
                .HasMaxLength(POST_TITLE_MAX_LENGTH)
                .IsRequired();
        }
    }
}
