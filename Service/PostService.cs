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

        public async Task Add(PostEntity entity)
        {
            entity.RegistrationDate = DateTime.UtcNow;
            await _postRepository.Add(entity);
        }

        public async Task<IEnumerable<PostEntity>> GetAll()
        {
            return await _postRepository.GetAll();
        }

        public async Task<PostEntity> GetById(int id)
        {
            return await _postRepository.GetById(id);
        }

        public async Task<IEnumerable<PostEntity>> GetWhere(Expression<Func<PostEntity, bool>> predicate)
        {
            return await _postRepository.GetWhere(predicate);
        }

        public async Task<bool> Remove(int id)
        {
            return await _postRepository.Remove(id);
        }

        public async Task Update(PostEntity entity)
        {
            await _postRepository.Update(entity);
        }
    }
}
