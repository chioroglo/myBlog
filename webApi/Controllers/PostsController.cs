using API.Controllers.Base;
using AutoMapper;
using Common.Dto.Paging.CursorPaging;
using Common.Dto.Paging.OffsetPaging;
using Common.Dto.Post;
using Common.Models;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;

namespace API.Controllers
{
    [Route("api/posts")]
    public class PostsController : AppBaseController
    {
        private readonly IPostService _postsService;
        private readonly IMapper _mapper;

        public PostsController(IPostService postsService, IMapper mapper,IUserService userService) : base(userService)
        {
            _postsService = postsService;
            _mapper = mapper;
        }


        [AllowAnonymous]
        [HttpGet("{postId:int}")]
        public async Task<PostModel> GetByIdWithUsernameAndTopicAndCommentsAsync(int postId, CancellationToken cancellationToken)
        {
            var post = await _postsService.GetByIdWithIncludeAsync(postId,cancellationToken,e => e.User,e => e.Topic,e => e.Comments);

            return _mapper.Map<PostModel>(post);
        }

        [HttpPost]
        public async Task<PostModel> CreatePostAsync([FromBody] PostDto postContent, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<Post>(postContent);

            await UpdateAuthorizedUserLastActivity(cancellationToken);
            request.UserId = GetCurrentUserId();
            await UpdateAuthorizedUserLastActivity(cancellationToken);

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
            await UpdateAuthorizedUserLastActivity(cancellationToken);

            await _postsService.UpdateAsync(request,cancellationToken);

            var updatedPost = await _postsService.GetByIdWithIncludeAsync(postId,cancellationToken,u => u.User);
            return _mapper.Map<PostModel>(updatedPost);
        }

        [HttpDelete("{postId:int}")]
        public async Task DeletePostByIdAsync(int postId, CancellationToken cancellationToken)
        {
            await UpdateAuthorizedUserLastActivity(cancellationToken);
            await _postsService.RemoveAsync(postId, issuerId: GetCurrentUserId(),cancellationToken);
        }


        [AllowAnonymous]
        [HttpPost("paginated-search-offset")]
        public async Task<OffsetPagedResult<PostModel>> GetOffsetPagedPostsWithUsersAsync([FromBody] OffsetPagedRequest pagedRequest, CancellationToken cancellationToken)
        {
            var response = await _postsService.GetOffsetPageAsync(pagedRequest,cancellationToken,e => e.User);

            return new OffsetPagedResult<PostModel>()
            {
                PageIndex = response.PageIndex,
                PageSize = response.PageSize,
                Total = response.Total,
                Items = response.Items.Select(e => _mapper.Map<PostModel>(e)).ToList()
            };
        }


        [AllowAnonymous]
        [HttpPost("paginated-search-cursor")]
        public async Task<CursorPagedResult<PostModel>> GetCursorPagedPostsWithUsersTopicsAndCommentsAsync([FromBody] CursorPagedRequest pagedRequest, CancellationToken cancellationToken)
        {
            var response = await _postsService.GetCursorPageAsync(pagedRequest, cancellationToken, e => e.User,e => e.Comments);

            return new CursorPagedResult<PostModel>()
            {
                PageSize = response.PageSize,
                Total = response.Total,
                Items = response.Items.Select(e => _mapper.Map<PostModel>(e)).ToList(),
                HeadElementId = response.HeadElementId,
                TailElementId = response.TailElementId
            };
        }
    }
}