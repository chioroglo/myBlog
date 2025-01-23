using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBlog.Data.Configurations.Abstract;
using MyBlog.Domain;

namespace MyBlog.Data.Configurations
{
    public class PostReactionEntityConfiguration : BaseEntityConfiguration<PostReaction>
    {
        public override void ConfigureNonPkProperties(EntityTypeBuilder<PostReaction> builder)
        {
            builder.ToTable(nameof(PostReaction));

            builder.HasAlternateKey(e => new { e.PostId, e.UserId });
        }
    }
}