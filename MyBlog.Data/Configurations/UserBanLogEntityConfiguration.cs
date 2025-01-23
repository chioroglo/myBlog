using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBlog.Common.Validation;
using MyBlog.Data.Configurations.Abstract;
using MyBlog.Domain;

namespace MyBlog.Data.Configurations;

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