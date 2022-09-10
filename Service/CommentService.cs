using AutoMapper;
using DAL.Repositories.Abstract;
using Domain;
using Service.Abstract;

namespace Service
{
    public class CommentService : ICommentService
    {
        private ICommentRepository _commentRepository;
        private IMapper _mapper;


        public CommentService(ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        public async Task Add(Comment entity)
        {
            await _commentRepository.AddAsync(entity);
            await _commentRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Comment>> GetAll()
        {
            return await _commentRepository.GetAllAsync();
        }

        public async Task<Comment> GetById(int id)
        {
            return await _commentRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Comment>> GetCommentsByPostId(int postId)
        {
            IEnumerable<Comment> comments;
            try
            {
                comments = await _commentRepository.GetWhereAsync(e => e.PostId == postId);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }

            return comments;
        }

        public async Task<bool> Remove(int id,int issuerId)
        {
            var wasSuccessfullyDeleted = await _commentRepository.RemoveAsync(id);

            if (wasSuccessfullyDeleted)
            {
                await _commentRepository.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> Update(Comment entity)
        {
            int commentId = entity.Id;

            var comment = await _commentRepository.GetByIdAsync(commentId);

            if (comment == null)
            {
                return false;
            }

            if (entity.UserId != comment.UserId)
            {
                return false;
            }

            comment.Content = entity.Content;

            try
            {
                await _commentRepository.UpdateAsync(comment);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }

            await _commentRepository.SaveChangesAsync();
            return true;
        }
    }
}