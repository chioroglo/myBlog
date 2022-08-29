using DAL.Configurations.Abstract;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class CommentEntityConfiguration : BaseEntityConfiguration<CommentEntity>
    {
        public override void ConfigureNonPkProperties(EntityTypeBuilder<CommentEntity> builder)
        {
            builder.HasOne(e => e.User)
                .WithMany(e => e.Comments)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(e => e.Post)
                .WithMany(e => e.Comments)
                .HasForeignKey(e => e.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(e => e.Content).IsRequired();
        }
    }
}
