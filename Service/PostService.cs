using Common.Exceptions;
using Common.Models.Pagination;
using DAL.Repositories.Abstract;
using Domain;
using Service.Abstract;

namespace Service
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task Add(Post request,CancellationToken cancellationToken)
        {
            if (await _postRepository.GetByTitleAsync(request.Title,cancellationToken) != null)
            {
                throw new ValidationException("This title is occupied");
            }

            await _postRepository.AddAsync(request,cancellationToken);
        }

        public async Task<IEnumerable<Post>> GetAllAsync(CancellationToken cancellationToken)
        {
            var result = await _postRepository.GetAllAsync(cancellationToken);
            
            return result;
        }

        public async Task<Post> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _postRepository.GetByIdAsync(id,cancellationToken);

            if (result == null)
            {
                throw new ValidationException($"{nameof(Post)} of ID: {id} does not exist");
            }

            return result;
        }

        public async Task RemoveAsync(int postId,int issuerId, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetByIdAsync(postId,cancellationToken);

            if (post == null)
            {
                throw new ValidationException($"{nameof(Post)} of ID: {postId} does not exist");
            }

            if (post.UserId != issuerId)
            {
                throw new ValidationException($"This {nameof(Post)} does not belong to authorized user");
            }

            await _postRepository.RemoveAsync(postId,cancellationToken);
        }

        public async Task UpdateAsync(Post request, CancellationToken cancellationToken)
        {
            int postId = request.Id;

            var post = await _postRepository.GetByIdAsync(postId,cancellationToken);

            if (post == null)
            {
                throw new ValidationException($"Post of postId {postId} was not found");
            }

            if (request.UserId != post.UserId)
            {
                throw new ValidationException($"Authorized user has no priveleges to edit this post postID:{postId}");
            }

            post.Title = request.Title;
            post.Content = request.Content;

            _postRepository.Update(post,cancellationToken);
            //await _postRepository.SaveChangesAsync(cancellationToken);
        }

        public async Task<PaginatedResult<Post>> GetPageAsync(PagedRequest query, CancellationToken cancellationToken)
        {
            var pagedPosts = await _postRepository.GetPagedData(query,cancellationToken);

            return pagedPosts;
        }
    }
}