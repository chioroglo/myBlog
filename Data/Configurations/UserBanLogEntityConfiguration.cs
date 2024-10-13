using Common.Validation;
using DAL.Configurations.Abstract;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations;

public class UserBanLogEntityConfiguration : BaseEntityConfiguration<UserBanLog>
{
    public override void ConfigureNonPkProperties(EntityTypeBuilder<UserBanLog> builder)
    {
        builder.ToTable(nameof(UserBanLog));

        builder.Property(e => e.Reason)
            .HasMaxLength(EntityConfigurationConstants.IndexableNvarcharLengthLimit);

        builder.HasOne(e => e.User)
            .WithMany(e => e.BanLogs)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}