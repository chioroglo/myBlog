using DAL.Configurations.Abstract;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class PostReactionEntityConfiguration : BaseEntityConfiguration<PostReaction>
    {
        public override void ConfigureNonPkProperties(EntityTypeBuilder<PostReaction> builder)
        {
            builder.ToTable(nameof(PostReaction));

            builder.Property(e => e.ReactionType);
        }
    }
}
