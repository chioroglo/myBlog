using DAL.Configurations.Abstract;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class PostEntityConfiguration : BaseEntityConfiguration<PostEntity>
    {
        public override void ConfigureNonPkProperties(EntityTypeBuilder<PostEntity> builder)
        {
            builder.HasMany(e => e.Comments).
                WithOne(e => e.Post).
                HasForeignKey(e => e.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(e => e.User).
                WithMany(e => e.Posts).
                OnDelete(DeleteBehavior.NoAction);

            builder.Property(e => e.Content).IsRequired();

        }
    }
}
