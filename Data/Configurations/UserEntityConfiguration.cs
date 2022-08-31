using DAL.Configurations.Abstract;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Entities.EntityConfigurationConstants;

namespace DAL.Configurations
{
    public class UserEntityConfiguration : BaseEntityConfiguration<UserEntity>
    {
        public override void ConfigureNonPkProperties(EntityTypeBuilder<UserEntity> builder)
        {
            builder.Property(e => e.Username).
                IsRequired().
                HasMaxLength(USERNAME_MAX_LENGTH);

            builder.Property(e => e.FirstName)
                .HasMaxLength(FIRSTNAME_LASTNAME_MAX_LENGTH);

            builder.Property(e => e.LastName)
                .HasMaxLength(FIRSTNAME_LASTNAME_MAX_LENGTH);

            builder.Property(e => e.Password).IsRequired();

            builder.Property(e => e.LastActivity).
                HasDefaultValueSql("GETUTCDATE()").IsRequired();
        }
    }
}
