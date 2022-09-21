using DAL.Repositories.Abstract;
using Domain;
using Domain.Models.Pagination;
using Service.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Service
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task Add(Post request)
        {
            if (await _postRepository.GetByTitleAsync(request.Title) != null)
            {
                throw new ValidationException("This title is occupied");
            }

            _postRepository.Add(request);
            await _postRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Post>> GetAll()
        {
            var result = await _postRepository.GetAllAsync();
            
            return result;
        }

        public async Task<Post> GetById(int id)
        {
            var result = await _postRepository.GetByIdAsync(id);

            if (result == null)
            {
                throw new ValidationException($"{nameof(Post)} of ID: {id} does not exist");
            }

            return result;
        }

        public async Task Remove(int postId,int issuerId)
        {
            var post = await _postRepository.GetByIdAsync(postId);

            if (post == null)
            {
                throw new ValidationException($"{nameof(Post)} of ID: {postId} does not exist");
            }

            if (post.UserId != issuerId)
            {
                throw new ValidationException($"This {nameof(Post)} does not belong to authorized user");
            }

            await _postRepository.RemoveAsync(postId);
            await _postRepository.SaveChangesAsync();
        }

        public async Task Update(Post request)
        {
            int postId = request.Id;

            var post = await _postRepository.GetByIdAsync(postId);

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

            _postRepository.Update(post);
            await _postRepository.SaveChangesAsync();
        }

        public async Task<PaginatedResult<Post>> GetPage(PagedRequest query)
        {
            var pagedPosts = await _postRepository.GetPagedData(query);

            return pagedPosts;
        }
    }
}
