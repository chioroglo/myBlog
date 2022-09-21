using DAL.Repositories.Abstract;
using Domain;
using Domain.Exceptions;
using Domain.Models.Pagination;
using Service.Abstract;

namespace Service
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;


        public CommentService(ICommentRepository commentRepository,IPostRepository postRepository)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
        }

        public async Task Add(Comment entity)
        {
            var post = _postRepository.GetByIdAsync(entity.PostId);

            if (post == null)
            {
                throw new ValidationException($"{nameof(Post)} of ID: {entity.PostId} does not exist");
            }

            _commentRepository.Add(entity);
            await _commentRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Comment>> GetAll()
        {
            return await _commentRepository.GetAllAsync();
        }

        public async Task<Comment> GetById(int id)
        {
            var comment =  await _commentRepository.GetByIdWithIncludeAsync(id, e => e.User);

            if (comment == null)
            {
                throw new ValidationException($"{nameof(Comment)} of ID: {id} does not exist");
            }

            return comment;
        }

        public async Task<IEnumerable<Comment>> GetCommentsByPostId(int postId)
        {
            var comments = await _commentRepository.GetByPostIdIncludeUserAsync(postId);
            return comments;
        }


        public async Task Remove(int id,int issuerId)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            
            if (comment == null)
            {
                throw new ValidationException($"{nameof(Comment)} of ID: {id} does not exist");
            }

            if (issuerId != comment.UserId)
            {
                throw new ValidationException($"Authorized {nameof(User)} of ID: {issuerId} has no access to this comment");
            }

            await _commentRepository.RemoveAsync(id);
            await _commentRepository.SaveChangesAsync();           
        }

        public async Task Update(Comment entity)
        {
            var comment = await _commentRepository.GetByIdAsync(entity.Id);

            if (comment == null)
            {
                throw new ValidationException($"{nameof(Comment)} of ID: {entity.Id} does not exist");
            }

            if (entity.UserId != comment.UserId)
            {
                throw new ValidationException("Authorized user has no access to this comment");
            }

            comment.Content = entity.Content;

            _commentRepository.Update(comment);
            await _commentRepository.SaveChangesAsync();
        }
        
        public async Task<PaginatedResult<Comment>> GetPage(PagedRequest query)
        {
            var pagedComments = await _commentRepository.GetPagedData(query);

            return pagedComments;
        }
    }
}