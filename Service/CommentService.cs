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

        public async Task Add(Comment entity,CancellationToken cancellationToken)
        {
            var post = _postRepository.GetByIdAsync(entity.PostId,cancellationToken);

            if (post == null)
            {
                throw new ValidationException($"{nameof(Post)} of ID: {entity.PostId} does not exist");
            }

            await _commentRepository.AddAsync(entity,cancellationToken);
            await _commentRepository.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Comment>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _commentRepository.GetAllAsync(cancellationToken);
        }

        public async Task<Comment> GetByIdAsync(int id,CancellationToken cancellationToken)
        {
            var comment =  await _commentRepository.GetByIdWithIncludeAsync(id,cancellationToken, e => e.User);

            if (comment == null)
            {
                throw new ValidationException($"{nameof(Comment)} of ID: {id} does not exist");
            }

            return comment;
        }

        public async Task<IEnumerable<Comment>> GetCommentsByPostId(int postId,CancellationToken cancellationToken)
        {
            var comments = await _commentRepository.GetByPostIdIncludeUserAsync(postId,cancellationToken);
            return comments;
        }


        public async Task RemoveAsync(int id,int issuerId,CancellationToken cancellationToken)
        {
            var comment = await _commentRepository.GetByIdAsync(id,cancellationToken);
            
            if (comment == null)
            {
                throw new ValidationException($"{nameof(Comment)} of ID: {id} does not exist");
            }

            if (issuerId != comment.UserId)
            {
                throw new ValidationException($"Authorized {nameof(User)} of ID: {issuerId} has no access to this comment");
            }

            await _commentRepository.RemoveAsync(id,cancellationToken);
            await _commentRepository.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Comment entity,CancellationToken cancellationToken)
        {
            var comment = await _commentRepository.GetByIdAsync(entity.Id,cancellationToken);

            if (comment == null)
            {
                throw new ValidationException($"{nameof(Comment)} of ID: {entity.Id} does not exist");
            }

            if (entity.UserId != comment.UserId)
            {
                throw new ValidationException("Authorized user has no access to this comment");
            }

            comment.Content = entity.Content;

            _commentRepository.Update(comment,cancellationToken);
            await _commentRepository.SaveChangesAsync(cancellationToken);
        }
        
        public async Task<PaginatedResult<Comment>> GetPageAsync(PagedRequest query, CancellationToken cancellationToken)
        {
            var pagedComments = await _commentRepository.GetPagedData(query,cancellationToken);

            return pagedComments;
        }
    }
}