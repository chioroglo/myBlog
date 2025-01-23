using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBlog.Data.Configurations.Abstract;
using MyBlog.Domain;

namespace MyBlog.Data.Configurations
{
    public class AvatarEntityConfiguration : BaseEntityConfiguration<Avatar>
    {
        public override void ConfigureNonPkProperties(EntityTypeBuilder<Avatar> builder)
        {
            builder.ToTable(nameof(Avatar));

            builder.Property(e => e.BlobName)
                .IsRequired();
        }
    }
}