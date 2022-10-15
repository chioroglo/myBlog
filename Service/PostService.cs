using Common.Dto.Paging.CursorPaging;
using Common.Dto.Paging.OffsetPaging;
using Common.Exceptions;
using DAL.Repositories.Abstract;
using Domain;
using Service.Abstract;
using System.Linq.Expressions;

namespace Service
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly ITopicRepository _topicRepository;

        public PostService(IPostRepository postRepository, ITopicRepository topicRepository)
        {
            _postRepository = postRepository;
            _topicRepository = topicRepository;
        }

        public async Task Add(Post request,CancellationToken cancellationToken)
        {
            if (await _postRepository.GetByTitleAsync(request.Title, cancellationToken) != null)
            {
                throw new ValidationException("This title is occupied");
            }

            await CheckIncomingTopicAndAddNewIfDoesNotExist(request,cancellationToken);

            await _postRepository.AddAsync(request,cancellationToken);
        }

        public async Task<IEnumerable<Post>> GetAllAsync(CancellationToken cancellationToken)
        {
            var result = await _postRepository.GetAllAsync(cancellationToken);
            
            return result;
        }

        public async Task<Post> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _postRepository.GetByIdAsync(id,cancellationToken);

            if (result == null)
            {
                throw new ValidationException($"{nameof(Post)} of ID: {id} does not exist");
            }

            return result;
        }

        public async Task RemoveAsync(int postId,int issuerId, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetByIdAsync(postId,cancellationToken);

            if (post == null)
            {
                throw new ValidationException($"{nameof(Post)} of ID: {postId} does not exist");
            }

            if (post.UserId != issuerId)
            {
                throw new ValidationException($"This {nameof(Post)} does not belong to authorized user");
            }

            await _postRepository.RemoveAsync(postId,cancellationToken);
        }

        public async Task UpdateAsync(Post request, CancellationToken cancellationToken)
        {
            int postId = request.Id;

            var post = await _postRepository.GetByIdAsync(postId,cancellationToken);

            if (post == null)
            {
                throw new ValidationException($"{nameof(Post)} of {nameof(Post)}Id {postId} was not found");
            }

            if (request.UserId != post.UserId)
            {
                throw new ValidationException($"Authorized user has no priveleges to edit this {nameof(Post)} postID:{postId}");
            }

            post.Title = request.Title;
            post.Content = request.Content;

            if (request.Topic != null)
            {
                await CheckIncomingTopicAndAddNewIfDoesNotExist(request, cancellationToken);
            }

            _postRepository.Update(post,cancellationToken);
        }

        public async Task<OffsetPagedResult<Post>> GetOffsetPageAsync(OffsetPagedRequest query, CancellationToken cancellationToken, params Expression<Func<Post, object>>[] includeProperties)
        {
            var pagedPosts = await _postRepository.GetPagedData(query,cancellationToken,includeProperties);

            return pagedPosts;
        }

        public async Task<Post> GetByIdWithIncludeAsync(int id, CancellationToken cancellationToken, params Expression<Func<Post, object>>[] includeProperties)
        {
            var post = await _postRepository.GetByIdWithIncludeAsync(id, cancellationToken, includeProperties);

            return post ?? throw new ValidationException($"{nameof(Post)} of ID: {id} does not exist");
        }

        public async Task<CursorPagedResult<Post>> GetCursorPageAsync(CursorPagedRequest query, CancellationToken cancellationToken, params Expression<Func<Post, object>>[] includeProperties)
        {
            var pagedPosts = await _postRepository.GetCursorPagedData(query, cancellationToken, includeProperties);

            return pagedPosts;
        }

        private async Task<bool> TopicOfNameDoesNotExist(string name, CancellationToken cancellationToken)
        {
            var result = (await _topicRepository.GetWhereAsync(e => e.Name == name, cancellationToken)).ToList();

            return result.Count == 0;
        }

        private async Task CheckIncomingTopicAndAddNewIfDoesNotExist(Post request, CancellationToken cancellationToken)
        {
            if (request.Topic != null)
            {
                if (await TopicOfNameDoesNotExist(request.Topic.Name, cancellationToken))
                {
                    await _topicRepository.AddAsync(new Topic() { Name = request.Topic.Name }, cancellationToken);

                    await _topicRepository.SaveChangesAsync();
                };

                var topic = (await _topicRepository.GetWhereAsync(e => e.Name == request.Topic.Name, cancellationToken)).FirstOrDefault();

                request.Topic = topic;
            }
        }
    }
}