using AutoMapper;
using DAL.Repositories.Abstract;
using Domain;
using Domain.Models;
using Service.Abstract;
using Service.Exceptions;

namespace Service
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public PostService(IPostRepository postRepository,IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public async Task Add(PostModel request)
        {
            if (await _postRepository.GetByTitle(request.Title) != null)
            {
                throw new ValidationException("This title is occupied");
            }

            var entity = _mapper.Map<Post>(request);

            await _postRepository.AddAsync(entity);
            await _postRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<PostModel>> GetAll()
        {
            var result = await _postRepository.GetAllAsync();

            var mappedResult = result.Select(e => _mapper.Map<PostModel>(e));
            
            return mappedResult;
        }

        public async Task<PostModel> GetById(int id)
        {
            var result = await _postRepository.GetByIdAsync(id);

            var mappedResult = _mapper.Map<PostModel>(result);
            return mappedResult;
        }

        public async Task<bool> Remove(int postId,int issuerId)
        {
            var post = await _postRepository.GetByIdAsync(postId);

            if (post == null)
            {
                throw new ValidationException($"Post of id {postId} does not exists");
            }
            if (post.UserId != issuerId)
            {
                throw new ValidationException("This post does not belong to authorized user");
            }

            await _postRepository.RemoveAsync(postId);
            await _postRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Update(PostModel request)
        {
            int postId = request.UserId;

            var post = await _postRepository.GetByIdAsync(postId);

            if (post == null)
            {
                throw new ValidationException($"Post of postId {postId} was not found");
            }
            if (request.AuthorId != post.UserId)
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
