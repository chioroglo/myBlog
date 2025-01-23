using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBlog.Data.Configurations.Abstract;
using MyBlog.Domain;
using static MyBlog.Common.Validation.EntityConfigurationConstants;

namespace MyBlog.Data.Configurations;

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