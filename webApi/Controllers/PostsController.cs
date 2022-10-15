using API.Controllers.Base;
using AutoMapper;
using Common.Dto.Post;
using Common.Models;
using Common.Models.Pagination;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;

namespace API.Controllers
{
    [Route("api/posts")]
    public class PostsController : AppBaseController
    {
        private readonly IPostService _postsService;
        private readonly IMapper _mapper;

        public PostsController(IPostService postsService,IMapper mapper)
        {
            _postsService = postsService;
            _mapper = mapper;
        }

        [HttpGet("{postId:int}")]
        public async Task<PostModel> GetByIdWithUsernameAsync(int postId, CancellationToken cancellationToken)
        {
            var post = await _postsService.GetByIdWithIncludeAsync(postId,cancellationToken,e => e.User);

            return _mapper.Map<PostModel>(post);
        }

        [HttpPost]
        public async Task<PostModel> CreatePostAsync([FromBody] PostDto postContent, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<Post>(postContent);
            request.UserId = GetCurrentUserId();

            await _postsService.Add(request,cancellationToken);

            request.RegistrationDate = DateTime.UtcNow;
            return _mapper.Map<PostModel>(request);
        }

        [HttpPut("{postId:int}")]
        public async Task<PostModel> UpdatePostByIdAsync(int postId,[FromBody] PostDto post, CancellationToken cancellationToken)
        {

            var request = _mapper.Map<Post>(post);
            request.Id = postId;
            request.UserId = GetCurrentUserId();

            await _postsService.UpdateAsync(request,cancellationToken);

            var updatedPost = await _postsService.GetByIdAsync(postId,cancellationToken);
            return _mapper.Map<PostModel>(updatedPost);
        }

        [HttpDelete("{postId:int}")]
        public async Task DeletePostByIdAsync(int postId, CancellationToken cancellationToken)
        {
            await _postsService.RemoveAsync(postId, issuerId: GetCurrentUserId(),cancellationToken);
        }

        [HttpPost("paginated-search")]
        public async Task<PaginatedResult<PostModel>> GetPagedPostsWithUsersAsync([FromBody] PagedRequest pagedRequest, CancellationToken cancellationToken)
        {
            var response = await _postsService.GetPageAsync(pagedRequest,cancellationToken,e => e.User);

            return new PaginatedResult<PostModel>()
            {
                PageIndex = response.PageIndex,
                PageSize = response.PageSize,
                Total = response.Total,
                Items = response.Items.Select(e => _mapper.Map<PostModel>(e)).ToList()
            };
        }
    }
}