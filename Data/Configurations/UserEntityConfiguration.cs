using DAL.Configurations.Abstract;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static Domain.Validation.EntityConfigurationConstants;

namespace DAL.Configurations
{
    public class UserEntityConfiguration : BaseEntityConfiguration<User>
    {
        public override void ConfigureNonPkProperties(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User));


            builder.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(USER_USERNAME_MAX_LENGTH);

            builder.HasIndex(e => new { e.Id, e.Username })
                .IsUnique();

            builder.Property(e => e.FirstName)
                .HasMaxLength(USER_FIRSTNAME_LASTNAME_MAX_LENGTH);

            builder.Property(e => e.LastName)
                .HasMaxLength(USER_FIRSTNAME_LASTNAME_MAX_LENGTH);

            builder.Property(e => e.Password)
                .IsRequired();

            builder.Property(e => e.LastActivity)
                .HasDefaultValueSql("GETUTCDATE()")
                .IsRequired();

            builder.HasMany(e => e.Posts)
                .WithOne(e => e.User);

            builder.HasMany(e => e.Comments)
                .WithOne(e => e.User)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(e => e.Avatar)
                .WithOne(e => e.User);
        }
    }
}