using Common.Dto.Paging.OffsetPaging;
using Common.Exceptions;
using DAL.Repositories.Abstract;
using Domain;
using Service.Abstract;
using System.Linq.Expressions;

namespace Service
{
    public class TopicService : ITopicService
    {
        private readonly ITopicRepository _topicRepository;

        public TopicService(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
        }

        public async Task Add(Topic entity, CancellationToken cancellationToken)
        {
            if (await ExistsTopicOfNameAsync(entity.Name,cancellationToken))
            {
                throw new ValidationException("This topic name is occupied");
            }

            await _topicRepository.AddAsync(entity, cancellationToken);
        }

        public async Task<IEnumerable<Topic>> GetAllAsync(CancellationToken cancellationToken)
        {
            var result = await _topicRepository.GetAllAsync(cancellationToken);

            return result;
        }

        public async Task<Topic> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _topicRepository.GetByIdAsync(id, cancellationToken);

            if (result == null)
            {
                throw new ValidationException($"{nameof(Topic)} of ID: {id} does not exist");
            }

            return result;
        }

        public async Task<Topic> GetByIdWithIncludeAsync(int id, CancellationToken cancellationToken, params Expression<Func<Topic, object>>[] includeProperties)
        {
            var result = await _topicRepository.GetByIdWithIncludeAsync(id, cancellationToken, includeProperties);

            return result ?? throw new ValidationException($"{nameof(Topic)} of ID: {id} does not exist");
        }

        public Task<OffsetPagedResult<Topic>> GetOffsetPageAsync(OffsetPagedRequest query, CancellationToken cancellationToken, params Expression<Func<Topic, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(int id, int issuerId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Topic entity, CancellationToken cancellationToken)
        {
            int topicId = entity.Id;

            var topic = await _topicRepository.GetByIdAsync(topicId,cancellationToken);

            if (topic == null)
            {
                throw new ValidationException($"{nameof(Topic)} of {nameof(Topic)}Id {topicId} was not found");
            }

            topic.Name = entity.Name;

            _topicRepository.Update(topic,cancellationToken);
        }

        private async Task<bool> ExistsTopicOfNameAsync(string name,CancellationToken cancellationToken)
        {
            var result = (await _topicRepository.GetWhereAsync(e => e.Name == name,cancellationToken)).ToList();

            return result.Count != 0;
        }
    }
}
