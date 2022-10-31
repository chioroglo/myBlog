using Domain;

namespace Service.Abstract
{
    public interface IPostReactionService : IEntityService<PostReaction>
    {
        Task<IEnumerable<PostReaction>> GetByPostId(int postId, CancellationToken cancellationToken);

        Task RemoveByPostId(int issuerId,int postId, CancellationToken cancellationToken);
    }
}
