using API.Controllers.Base;
using AutoMapper;
using Domain;
using Domain.Dto.PostReaction;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;

namespace API.Controllers
{
    public class PostReactionController : AppBaseController
    {
        private readonly IPostReactionService _postReactionService;
        private readonly IMapper _mapper;

        public PostReactionController(IPostReactionService postReactionService, IMapper mapper)
        {
            _postReactionService = postReactionService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<PostReactionDto>> GetAll()
        {
            var result = await _postReactionService.GetAll();

            return result.Select(r => _mapper.Map<PostReactionDto>(r));
        }


        [Route("{postId:int}")]
        [HttpGet]
        public async Task<IEnumerable<PostReactionDto>> GetByPostId(int postId)
        {
            var result = await _postReactionService.GetByPostId(postId);

            return result.Select(r => _mapper.Map<PostReactionDto>(r));
        }


        [HttpPost]
        public async Task<PostReactionDto> CreateReaction([FromBody] PostReactionDto dto)
        {
            dto.UserId = GetCurrentUserId();

            var request = _mapper.Map<PostReaction>(dto);

            await _postReactionService.Add(request);

            return dto;
        }

        [HttpPut]
        public async Task<PostReactionDto> EditReaction([FromBody] PostReactionDto dto)
        {
            dto.UserId = GetCurrentUserId();

            var request = _mapper.Map<PostReaction>(dto);

            await _postReactionService.Update(request);

            return dto;
        }

        [HttpDelete("{postId:int}")]
        public async Task RemoveReactionFromPost(int postId)
        {
            await _postReactionService.Remove(postId, GetCurrentUserId());
        }
    }
}