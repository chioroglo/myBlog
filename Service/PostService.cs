using DAL.Repositories.Abstract;
using Domain;
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
            if (await _postRepository.GetByTitle(request.Title) != null)
            {
                throw new ValidationException("This title is occupied");
            }

            
            await _postRepository.AddAsync(request);
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

            return result;
        }

        public async Task<bool> Remove(int postId,int issuerId)
        {
            
            var post = await _postRepository.GetByIdAsync(postId);

            if (post.UserId != issuerId)
            {
                throw new ValidationException("This post does not belong to authorized user");
            }

            await _postRepository.RemoveAsync(postId);
            await _postRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Update(Post request)
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

            await _postRepository.UpdateAsync(post);
            await _postRepository.SaveChangesAsync();

            return true;
        }
    }
}
