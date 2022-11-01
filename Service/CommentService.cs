using Common.Dto.Paging.OffsetPaging;
using Common.Exceptions;
using DAL.Repositories.Abstract;
using Domain;
using Service.Abstract;
using System.Linq.Expressions;

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

        public async Task<Comment> Add(Comment entity,CancellationToken cancellationToken)
        {
            var post = _postRepository.GetByIdAsync(entity.PostId,cancellationToken);

            if (post == null)
            {
                throw new ValidationException($"{nameof(Post)} of ID: {entity.PostId} does not exist");
            }

            return await _commentRepository.AddAsync(entity,cancellationToken);
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
            var comments = await _commentRepository.GetByPostIdIncludeUserAndPostAsync(postId,cancellationToken);

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
        }
        
        public async Task<OffsetPagedResult<Comment>> GetOffsetPageAsync(OffsetPagedRequest query, CancellationToken cancellationToken, params Expression<Func<Comment, object>>[] includeProperties)
        {
            var pagedComments = await _commentRepository.GetPagedData(query,cancellationToken,includeProperties);

            return pagedComments;
        }

        public async Task<Comment> GetByIdWithIncludeAsync(int id, CancellationToken cancellationToken, params Expression<Func<Comment, object>>[] includeProperties)
        {
            var comment = await _commentRepository.GetByIdWithIncludeAsync(id, cancellationToken, includeProperties);

            return comment ?? throw new ValidationException($"{nameof(Comment)} of ID: {id} does not exist");
        }
    }
}