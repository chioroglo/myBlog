using Common.Exceptions;
using Common.Models.Pagination;
using DAL.Repositories.Abstract;
using Domain;
using Service.Abstract;

namespace Service
{
    public class PostReactionService : IPostReactionService
    {
        private readonly IPostReactionRepository _postReactionRepository;
        private readonly IPostRepository _postRepository;

        public PostReactionService(IPostReactionRepository postReactionRepository, IPostRepository postRepository)
        {
            _postReactionRepository = postReactionRepository;
            _postRepository = postRepository;
        }

        public async Task<IEnumerable<PostReaction>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _postReactionRepository.GetAllAsync(cancellationToken);
        }

        public async Task<PostReaction> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var reaction = await _postReactionRepository.GetByIdAsync(id,cancellationToken);

            if (reaction == null)
            {
                throw new ValidationException($"{nameof(Comment)} of ID: {id} does not exist");
            }

            return reaction;
        }

        public async Task<IEnumerable<PostReaction>> GetByPostId(int postId,CancellationToken cancellationToken)
        {
            if (await PostDoesNotExistAsync(postId,cancellationToken))
            {
                throw new ValidationException($"{nameof(Post)} of ID: {postId} does not exist");
            }

            return await _postReactionRepository.GetWhereAsync(p => p.PostId == postId,cancellationToken);
        }

        public async Task<PaginatedResult<PostReaction>> GetPageAsync(PagedRequest query, CancellationToken cancellationToken)
        {
            return await _postReactionRepository.GetPagedData(query,cancellationToken);
        }
        
        public async Task Add(PostReaction entity, CancellationToken cancellationToken)
        {
            if (await PostDoesNotExistAsync(entity.PostId,cancellationToken))
            {
                throw new ValidationException($"{nameof(Post)} of ID: {entity.PostId} does not exist");
            }

            if (await ExistsSuchReactionAsync(entity.PostId,entity.UserId,cancellationToken))
            {
                throw new ValidationException($"{nameof(Post)}ID: {entity.PostId} already has found reaction from {nameof(User)}ID: {entity.UserId}");
            }

            await _postReactionRepository.AddAsync(entity,cancellationToken);
        }

        public async Task RemoveAsync(int reactionId, int issuerId, CancellationToken cancellationToken)
        {
            var reaction = await _postReactionRepository.GetByIdAsync(reactionId,cancellationToken);

            if (reaction == null)
            {
                throw new ValidationException($"Object ID : {reactionId} of {nameof(PostReaction)} does not exist");
            }

            if (reaction.UserId != issuerId)
            {
                throw new ValidationException($"This {nameof(PostReaction)} does not belong to authorized user");
            }

            await _postReactionRepository.RemoveAsync(reactionId,cancellationToken);
        }

        public async Task UpdateAsync(PostReaction entity, CancellationToken cancellationToken)
        {
            var found = await _postReactionRepository.GetWhereAsync(r => r.PostId == entity.PostId && r.UserId == entity.UserId,cancellationToken);

            var post = found.FirstOrDefault();

            if (post == null)
            {
                throw new ValidationException($"{nameof(PostReaction)} of ID:{entity.Id} does not exist");
            }

            post.ReactionType = entity.ReactionType;

            _postReactionRepository.Update(post,cancellationToken);
        }

        private async Task<bool> ExistsSuchReactionAsync(int postId, int userId, CancellationToken cancellationToken)
        {
            var requestedPostReactionsPostedByUser = await _postReactionRepository.GetWhereAsync(r => r.PostId == postId && r.UserId == userId,cancellationToken);

            return requestedPostReactionsPostedByUser.Any();
        }

        private async Task<bool> PostDoesNotExistAsync(int postId,CancellationToken cancellationToken)
        {
            var searchRequestForPost = await _postRepository.GetByIdAsync(postId,cancellationToken);

            return searchRequestForPost == null;
        }
    }
}
