using DAL.Configurations.Abstract;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Domain.Validation.EntityConfigurationConstants;

namespace DAL.Configurations
{
    public class PostEntityConfiguration : BaseEntityConfiguration<Post>
    {
        public override void ConfigureNonPkProperties(EntityTypeBuilder<Post> builder)
        {

            builder.ToTable(nameof(Post));

            builder.Property(e => e.Content)
                .HasMaxLength(POST_MAX_LENGTH)
                .IsRequired();

            builder.Property(e => e.Title)
                .HasMaxLength(POST_TITLE_MAX_LENGTH)
                .IsRequired();

            builder.HasMany(e => e.Comments)
                .WithOne(e => e.Post);

            builder.HasMany(e => e.Reactions)
                .WithOne(e => e.Post);
        }
    }
}
