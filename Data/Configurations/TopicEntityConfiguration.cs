using DAL.Configurations.Abstract;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Common.Validation.EntityConfigurationConstants;


namespace DAL.Configurations
{
    public class TopicEntityConfiguration : BaseEntityConfiguration<Topic>
    {
        public override void ConfigureNonPkProperties(EntityTypeBuilder<Topic> builder)
        {
            builder.ToTable(nameof(Topic));

            builder.Property(e => e.Name)
                .HasMaxLength(MaxTopicNameLength);
        }
    }
}
