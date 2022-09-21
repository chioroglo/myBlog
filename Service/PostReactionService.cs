using DAL.Repositories.Abstract;
using Domain;
using Domain.Exceptions;
using Domain.Models.Pagination;
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

        public async Task<IEnumerable<PostReaction>> GetAll()
        {
            return await _postReactionRepository.GetAllAsync();
        }

        public async Task<PostReaction> GetById(int id)
        {
            var reaction = await _postReactionRepository.GetByIdAsync(id);

            if (reaction == null)
            {
                throw new ValidationException($"{nameof(Comment)} of ID: {id} does not exist");
            }

            return reaction;
        }

        public async Task<IEnumerable<PostReaction>> GetByPostId(int postId)
        {
            if (await PostDoesNotExistAsync(postId))
            {
                throw new ValidationException($"{nameof(Post)} of ID: {postId} does not exist");
            }

            return await _postReactionRepository.GetWhereAsync(p => p.PostId == postId);
        }

        public async Task<PaginatedResult<PostReaction>> GetPage(PagedRequest query)
        {
            return await _postReactionRepository.GetPagedData(query);
        }
        
        public async Task Add(PostReaction entity)
        {
            if (await PostDoesNotExistAsync(entity.PostId))
            {
                throw new ValidationException($"{nameof(Post)} of ID: {entity.PostId} does not exist");
            }

            if (await ExistsSuchReactionAsync(entity.PostId,entity.UserId))
            {
                throw new ValidationException($"{nameof(Post)}ID: {entity.PostId} already has found from {nameof(User)}ID: {entity.UserId}");
            }

            _postReactionRepository.Add(entity);
            await _postReactionRepository.SaveChangesAsync();
        }

        public async Task Remove(int id, int issuerId)
        {
            var reaction = await _postReactionRepository.GetByIdAsync(id);

            if (reaction == null)
            {
                throw new ValidationException($"Object ID : {id} of {nameof(PostReaction)} does not exist");
            }

            if (reaction.UserId != issuerId)
            {
                throw new ValidationException($"This {nameof(PostReaction)} does not belong to authorized user");
            }

            await _postReactionRepository.RemoveAsync(id);
            await _postReactionRepository.SaveChangesAsync();
        }

        public async Task Update(PostReaction entity)
        {
            var found = await _postReactionRepository.GetWhereAsync(r => r.PostId == entity.PostId && r.UserId == entity.UserId);

            var post = found.FirstOrDefault();

            if (post == null)
            {
                throw new ValidationException($"{nameof(PostReaction)} of ID:{entity.Id} does not exist");
            }

            post.ReactionType = entity.ReactionType;

            _postReactionRepository.Update(post);
            await _postReactionRepository.SaveChangesAsync();
        }

        private async Task<bool> ExistsSuchReactionAsync(int postId, int userId)
        {
            var requestedPostReactionsPostedByUser = await _postReactionRepository.GetWhereAsync(r => r.PostId == postId && r.UserId == userId);

            return requestedPostReactionsPostedByUser.Any();
        }

        private async Task<bool> PostDoesNotExistAsync(int postId)
        {
            var searchRequestForPost = await _postRepository.GetByIdAsync(postId);

            return searchRequestForPost == null;
        }
    }
}
