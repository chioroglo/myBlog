using API.Controllers.Base;
using AutoMapper;
using Domain;
using Domain.Dto.PostReaction;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;

namespace API.Controllers
{
    [Route("api/reactions")]
    public class PostReactionController : AppBaseController
    {
        private readonly IPostReactionService _postReactionService;
        private readonly IMapper _mapper;

        public PostReactionController(IPostReactionService postReactionService, IMapper mapper)
        {
            _postReactionService = postReactionService;
            _mapper = mapper;
        }

        [Route("{postId:int}")]
        [HttpGet]
        public async Task<IEnumerable<PostReactionDto>> GetByPostId(int postId, CancellationToken cancellationToken)
        {
            var result = await _postReactionService.GetByPostId(postId,cancellationToken);

            return result.Select(r => _mapper.Map<PostReactionDto>(r));
        }


        [HttpPost]
        public async Task<PostReactionDto> CreateReaction([FromBody] PostReactionDto dto, CancellationToken cancellationToken)
        {
            dto.UserId = GetCurrentUserId();

            var request = _mapper.Map<PostReaction>(dto);

            await _postReactionService.Add(request,cancellationToken);

            return dto;
        }

        [HttpPut]
        public async Task<PostReactionDto> EditReaction([FromBody] PostReactionDto dto, CancellationToken cancellationToken)
        {
            dto.UserId = GetCurrentUserId();

            var request = _mapper.Map<PostReaction>(dto);

            await _postReactionService.Update(request,cancellationToken);

            return dto;
        }

        [HttpDelete("{postId:int}")]
        public async Task RemoveReactionByPostId(int postId, CancellationToken cancellationToken)
        {
            await _postReactionService.Remove(postId, GetCurrentUserId(),cancellationToken);
        }
    }
}