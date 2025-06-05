using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBlog.Data.Configurations.Abstract;
using MyBlog.Domain;
using static MyBlog.Common.Validation.EntityConfigurationConstants;

namespace MyBlog.Data.Configurations
{
    public class UserEntityConfiguration : BaseEntityConfiguration<User>
    {
        public override void ConfigureNonPkProperties(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User));

            builder.Property(e => e.Initials)
                .HasMaxLength(2)
                .IsRequired()
                .HasComputedColumnSql("""
                                      CASE 
                                          WHEN "FirstName" IS NOT NULL AND "LastName" IS NOT NULL 
                                              THEN SUBSTRING("FirstName" FROM 1 FOR 1) || SUBSTRING("LastName" FROM 1 FOR 1)
                                          ELSE SUBSTRING("Username" FROM 1 FOR 1)
                                      END
                                      """, stored: true);

            builder.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(UsernameMaxLength);

            builder.HasIndex(e => new { e.Id, e.Username })
                .IsUnique();

            builder.Property(e => e.FirstName)
                .HasMaxLength(UserFirstAndLastNameMaxLength);

            builder.Property(e => e.LastName)
                .HasMaxLength(UserFirstAndLastNameMaxLength);

            builder.Property(e => e.PasswordHash)
                .HasMaxLength(HashedPasswordLengthSha256)
                .IsRequired();

            builder.Property(e => e.RefreshToken)
                .HasMaxLength(RefreshTokenLength)
                .IsRequired(false);

            builder.HasIndex(e => new { e.RefreshToken })
                .IsUnique();

            builder.Property(e => e.LastActivity)
                .HasDefaultValueSql(CurrentTimestampUtcSql)
                .IsRequired();

            builder.HasMany(e => e.Posts)
                .WithOne(e => e.User);

            builder.HasMany(e => e.Comments)
                .WithOne(e => e.User)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(e => e.Avatar)
                .WithOne(e => e.User);

            builder.HasMany(e => e.PostReactions)
                .WithOne(e => e.User)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}