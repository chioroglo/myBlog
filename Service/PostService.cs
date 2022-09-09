using AutoMapper;
using DAL.Repositories.Abstract;
using Domain;
using Domain.Models;
using Service.Abstract;

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

        public async Task Add(PostModel viewModel)
        {
            var entity = _mapper.Map<Post>(viewModel);

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

        public async Task<bool> Remove(int id)
        {
            var wasSuccessfullyDeleted = await _postRepository.RemoveAsync(id);

            if (wasSuccessfullyDeleted)
            {
                await _postRepository.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> Update(PostModel request)
        {
            int postId = request.Id;

            var post = await _postRepository.GetByIdAsync(postId);

            if (post == null)
            {
                return false;
            }

            if (request.AuthorId != post.UserId)
            {
                return false;
            }

            post.Title = request.Title;
            post.Content = request.Content;

            try
            {
                await _postRepository.UpdateAsync(post);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }


            await _postRepository.SaveChangesAsync();

            return true;
        }
    }
}
