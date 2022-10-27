using API.Controllers.Base;
using AutoMapper;
using Common.Dto.PostReaction;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;

namespace API.Controllers
{
    [Route("api/reactions")]
    public class PostReactionController : AppBaseController
    {
        private readonly IPostReactionService _postReactionService;
        private readonly IMapper _mapper;

        public PostReactionController(IPostReactionService postReactionService, IMapper mapper, IUserService userService) : base(userService)
        {
            _postReactionService = postReactionService;
            _mapper = mapper;
        }


        [AllowAnonymous]
        [HttpGet("{postId:int}")]
        public async Task<IEnumerable<PostReactionDto>> GetByPostIdAsync(int postId, CancellationToken cancellationToken)
        {
            var result = await _postReactionService.GetByPostId(postId,cancellationToken);

            return result.Select(r => _mapper.Map<PostReactionDto>(r));
        }


        [HttpPost]
        public async Task<PostReactionDto> CreateReactionAsync
            ([FromBody] PostReactionDto dto, CancellationToken cancellationToken)
        {
            dto.UserId = GetCurrentUserId();

            var request = _mapper.Map<PostReaction>(dto);

            await _postReactionService.Add(request,cancellationToken);

            return dto;
        }

        [HttpPut]
        public async Task<PostReactionDto> EditReactionAsync([FromBody] PostReactionDto dto, CancellationToken cancellationToken)
        {
            dto.UserId = GetCurrentUserId();

            var request = _mapper.Map<PostReaction>(dto);

            await _postReactionService.UpdateAsync(request,cancellationToken);

            return dto;
        }

        [HttpDelete("{postId:int}")]
        public async Task RemoveReactionByPostIdAsync(int postId, CancellationToken cancellationToken)
        {
            await _postReactionService.RemoveAsync(postId, GetCurrentUserId(),cancellationToken);
        }
    }
}