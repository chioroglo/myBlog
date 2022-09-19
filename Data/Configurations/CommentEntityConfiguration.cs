using DAL.Configurations.Abstract;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Domain.Validation.EntityConfigurationConstants;

namespace DAL.Configurations
{
    public class CommentEntityConfiguration : BaseEntityConfiguration<Comment>
    {
        public override void ConfigureNonPkProperties(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable(nameof(Comment));

            builder.Property(e => e.Content)
                .HasMaxLength(COMMENT_MAX_LENGTH)
                .IsRequired();
        }
    }
}