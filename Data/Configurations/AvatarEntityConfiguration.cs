using DAL.Configurations.Abstract;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class AvatarEntityConfiguration : BaseEntityConfiguration<Avatar>
    {
        public override void ConfigureNonPkProperties(EntityTypeBuilder<Avatar> builder)
        {
            builder.ToTable(nameof(Avatar));

            builder.HasOne(e => e.User)
                .WithOne(e => e.Avatar);
        }
    }
}
