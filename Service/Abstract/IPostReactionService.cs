using Domain;

namespace Service.Abstract
{
    public interface IPostReactionService : IEntityService<PostReaction>
    {
        Task<IEnumerable<PostReaction>> GetByPostId(int postId);
    }
}
