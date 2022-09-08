using DAL.Configurations.Abstract;
using Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class AvatarEntityConfiguration : BaseEntityConfiguration<AvatarEntity>
    {
        public override void ConfigureNonPkProperties(EntityTypeBuilder<AvatarEntity> builder)
        {
            builder.HasOne(e => e.User)
                .WithOne(e => e.Avatar);
        }
    }
}
