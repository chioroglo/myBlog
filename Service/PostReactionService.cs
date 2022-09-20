using DAL.Repositories.Abstract;
using Domain;
using Domain.Models.Pagination;
using Service.Abstract;

namespace Service
{
    public class PostReactionService : IPostReactionService
    {
        private readonly IPostReactionRepository _postReactionRepository;

        public PostReactionService(IPostReactionRepository postReactionRepository)
        {
            _postReactionRepository = postReactionRepository;
        }

        public async Task Add(PostReaction entity)
        {
            await _postReactionRepository.AddAsync(entity);
        }

        public async Task<IEnumerable<PostReaction>> GetAll()
        {
            return await _postReactionRepository.GetAllAsync();
        }

        public async Task<PostReaction> GetById(int id)
        {
            return await _postReactionRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<PostReaction>> GetByPostId(int postId)
        {
            return await _postReactionRepository.GetWhereAsync(p => p.PostId == postId);
        }

        public async Task<PaginatedResult<PostReaction>> GetPage(PagedRequest query)
        {
            return await _postReactionRepository.GetPagedData(query);
        }

        public async Task<bool> Remove(int id, int issuerId)
        {
            await _postReactionRepository.RemoveAsync(id);

            return true;
        }

        public async Task<bool> Update(PostReaction entity)
        {
            await _postReactionRepository.UpdateAsync(entity);

            return true;
        }
    }
}
