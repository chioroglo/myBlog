using DAL.Configurations.Abstract;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Common.Validation.EntityConfigurationConstants;

namespace DAL.Configurations;

public class UserWarningEntityConfiguration : BaseEntityConfiguration<UserWarning>
{
    public override void ConfigureNonPkProperties(EntityTypeBuilder<UserWarning> builder)
    {
        builder.ToTable(nameof(UserWarning));

        builder.Property(e => e.Reason)
            .HasColumnType(Nvarchar)
            .HasMaxLength(IndexableNvarcharLengthLimit)
            .IsRequired();
    }
}