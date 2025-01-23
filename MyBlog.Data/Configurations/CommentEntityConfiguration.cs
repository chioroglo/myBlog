using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBlog.Data.Configurations.Abstract;
using MyBlog.Domain;
using static MyBlog.Common.Validation.EntityConfigurationConstants;

namespace MyBlog.Data.Configurations
{
    public class CommentEntityConfiguration : BaseEntityConfiguration<Comment>
    {
        public override void ConfigureNonPkProperties(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable(nameof(Comment));

            builder.Property(e => e.Content)
                .HasMaxLength(CommentMaxLength)
                .IsRequired();
        }
    }
}