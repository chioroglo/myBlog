using DAL.Configurations.Abstract;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Common.Validation.EntityConfigurationConstants;

namespace DAL.Configurations
{
    public class PasskeyEntityConfiguration : BaseEntityConfiguration<Passkey>
    {
        public override void ConfigureNonPkProperties(EntityTypeBuilder<Passkey> builder)
        {
            builder.ToTable(nameof(Passkey));

            builder.Property(p => p.CredentialId)
                .HasColumnType("VARCHAR")
                .HasMaxLength(IndexableVarcharLengthLimit)
                .IsRequired();

            builder.Property(p => p.PublicKey)
                .HasColumnType(Varchar)
                .HasMaxLength(IndexableVarcharLengthLimit)
                .IsRequired();
            
            builder.Property(p => p.CredentialType)
                .HasColumnType(Varchar)
                .HasMaxLength(IndexableVarcharLengthLimit)
                .IsRequired();

        }
    }
}