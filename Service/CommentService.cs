using DAL.Repositories.Abstract;
using Domain;
using Domain.Exceptions;
using Domain.Models.Pagination;
using Service.Abstract;

namespace Service
{
    public class CommentService : ICommentService
    {
        private ICommentRepository _commentRepository;
        private IPostRepository _postRepository;


        public CommentService(ICommentRepository commentRepository,IPostRepository postRepository)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
        }

        public async Task Add(Comment entity)
        {
            // check if post exists
            await _postRepository.GetByIdAsync(entity.PostId);

            await _commentRepository.AddAsync(entity);
            await _commentRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Comment>> GetAll()
        {
            return await _commentRepository.GetAllAsync();
        }

        public async Task<Comment> GetById(int id)
        {
            return await _commentRepository.GetByIdWithIncludeAsync(id, e => e.User);
        }

        public async Task<IEnumerable<Comment>> GetCommentsByPostId(int postId)
        {
            IEnumerable<Comment> comments = await _commentRepository.GetByPostIdIncludeUserAsync(postId);
            return comments;
        }


        public async Task<bool> Remove(int id,int issuerId)
        {
            var comment = await _commentRepository.GetByIdAsync(id);

            if (issuerId != comment.UserId)
            {
                throw new ValidationException("Authorized user has no access to this comment");
            }

            await _commentRepository.RemoveAsync(id);
            await _commentRepository.SaveChangesAsync();
            return true;
            
        }

        public async Task<bool> Update(Comment entity)
        {
            var comment = await _commentRepository.GetByIdAsync(entity.Id);

            if (entity.UserId != comment.UserId)
            {
                throw new ValidationException("Authorized user has no access to this comment");
            }

            comment.Content = entity.Content;

            await _commentRepository.UpdateAsync(comment);
            await _commentRepository.SaveChangesAsync();
            return true;
        }
        
        public async Task<PaginatedResult<Comment>> GetPage(PagedRequest query)
        {
            var pagedComments = await _commentRepository.GetPagedData(query);

            return pagedComments;
        }
    }
}