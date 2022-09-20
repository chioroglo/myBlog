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

            builder.HasAlternateKey(e => new { e.PostId, e.UserId });

            builder.Property(e => e.ReactionType);
        }
    }
}
