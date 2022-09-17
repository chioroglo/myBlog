using API.Controllers.Base;
using AutoMapper;
using Domain;
using Domain.Dto.Post;
using Domain.Models;
using Domain.Models.Pagination;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;

namespace webApi.Controllers
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

        [HttpGet]
        public async Task<IEnumerable<PostModel>> GetAll()
        {
            var posts = await _postsService.GetAll();

            
            return posts.Select(e => _mapper.Map<PostModel>(e));
        }

        [HttpGet("{postId:int}")]
        public async Task<PostModel> Get(int postId)
        {
            var post = await _postsService.GetById(postId);

            return _mapper.Map<PostModel>(post);
        }

        [HttpPost]
        public async Task<PostModel> CreatePost(PostDto postContent)
        {
            var request = _mapper.Map<Post>(postContent);
            request.UserId = GetCurrentUserId();

            await _postsService.Add(request);

            request.RegistrationDate = DateTime.UtcNow;
            return _mapper.Map<PostModel>(request);
        }

        [HttpPut("{postId:int}")]
        public async Task<PostModel> UpdatePost(int postId,[FromBody] PostDto post)
        {

            var request = _mapper.Map<Post>(post);
            request.Id = postId;
            request.UserId = GetCurrentUserId();

            await _postsService.Update(request);

            var updatedPost = await _postsService.GetById(postId);
            return _mapper.Map<PostModel>(updatedPost);
        }

        [HttpDelete("{postId:int}")]
        public async Task<IActionResult> Delete(int postId)
        {
            await _postsService.Remove(postId, issuerId: GetCurrentUserId());
            return Ok();
        }

        [HttpPost("paginated-search")]
        public async Task<PaginatedResult<PostModel>> GetPagedPosts(PagedRequest pagedRequest)
        {
            var response = await _postsService.GetPage(pagedRequest);

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