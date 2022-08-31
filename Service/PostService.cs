using AutoMapper;
using DAL.Repositories.Abstract;
using Domain;
using Entities;
using Service.Abstract;
using System.Linq.Expressions;

namespace Service
{
    public class PostService : IPostsService
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
            var entity = _mapper.Map<PostEntity>(viewModel);

            await _postRepository.Add(entity);
        }

        public async Task<IEnumerable<PostModel>> GetAll()
        {
            var result = await _postRepository.GetAll();

            var mappedResult = result.Select(e => _mapper.Map<PostModel>(e));
            
            return mappedResult;
        }

        public async Task<PostModel> GetById(int id)
        {
            var result = await _postRepository.GetById(id);

            var mappedResult = _mapper.Map<PostModel>(result);
            return mappedResult;
        }

        public async Task<bool> Remove(int id)
        {
            return await _postRepository.Remove(id);
        }

        public async Task Update(PostModel entity)
        {
            var mappedRequest = _mapper.Map<PostEntity>(entity);

            await _postRepository.Update(mappedRequest);
        }
    }
}
