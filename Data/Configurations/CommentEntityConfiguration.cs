using DAL.Configurations.Abstract;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static DAL.Configurations.EntityConfigurationConstants;

namespace DAL.Configurations
{
    public class CommentEntityConfiguration : BaseEntityConfiguration<Comment>
    {
        public override void ConfigureNonPkProperties(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable(nameof(Comment));
            // todo udalit noaction
            builder.HasOne(e => e.User)
                .WithMany(e => e.Comments)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(e => e.Post)
                .WithMany(e => e.Comments)
                .HasForeignKey(e => e.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(e => e.Content)
                .HasMaxLength(COMMENT_MAX_LENGTH)
                .IsRequired();
        }
    }
}