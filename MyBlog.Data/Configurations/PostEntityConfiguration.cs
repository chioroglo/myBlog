using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBlog.Data.Configurations.Abstract;
using MyBlog.Domain;
using static MyBlog.Common.Validation.EntityConfigurationConstants;

namespace MyBlog.Data.Configurations
{
    public class PostEntityConfiguration : BaseEntityConfiguration<Post>
    {
        public override void ConfigureNonPkProperties(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable(nameof(Post));

            builder.Property(e => e.Content)
                .HasMaxLength(PostMaxLength)
                .IsRequired();

            builder.Property(e => e.Title)
                .HasMaxLength(PostTitleMaxLength)
                .IsRequired();

            builder.HasMany(e => e.Comments)
                .WithOne(e => e.Post);

            builder.HasMany(e => e.Reactions)
                .WithOne(e => e.Post);

            builder.Property(e => e.Topic)
                .HasMaxLength(MaxTopicNameLength);
        }
    }
}