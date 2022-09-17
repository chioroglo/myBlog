using Domain.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DAL.Configurations.Abstract
{
    public abstract class BaseEntityConfiguration<TEntity>
        : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
    {
        private void ConfigurePkAndRegistrationDate(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("GETUTCDATE()")
                .IsRequired();
        }

        public abstract void ConfigureNonPkProperties(EntityTypeBuilder<TEntity> builder);

        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            ConfigurePkAndRegistrationDate(builder);
            ConfigureNonPkProperties(builder);
        }
    }
}