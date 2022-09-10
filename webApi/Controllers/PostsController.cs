using API.Controllers.Base;
using AutoMapper;
using Domain.Dto.Post;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IEnumerable<PostModel>> Get()
        {
            var posts = await _postsService.GetAll();

            return posts.ToList();
        }

        [HttpGet("{postId:int}")]
        public async Task<PostModel> Get(int postId)
        {
            PostModel post = await _postsService.GetById(postId);

            return post;
        }

        [HttpPost]
        public async Task<PostModel> CreatePost(PostDto postContent)
        {
            PostModel postRequest = _mapper.Map<PostModel>(postContent);
            postRequest.AuthorId = GetCurrentUserId();

            await _postsService.Add(postRequest);

            postRequest.RegistrationDate = DateTime.UtcNow;
            return postRequest;
        }

        [HttpPut("{postId:int}")]
        public async Task<PostModel> UpdatePost(int postId,[FromBody] PostDto post)
        {

            PostModel updateRequest = _mapper.Map<PostModel>(post);

            updateRequest.Id = postId;
            updateRequest.AuthorId = GetCurrentUserId();

            await _postsService.Update(updateRequest);

            var updatedPost = await _postsService.GetById(postId);

            return updatedPost;
        }

        [HttpDelete("{postId:int}")]
        public async Task<IActionResult> Delete(int postId)
        {
            //      var post = await _postsService.GetById(postId);
            //      var currentUserId = GetCurrentUserId();
            //
            //      if (post.AuthorId == currentUserId)
            //          await _postsService.Remove(postId);

            await _postsService.Remove(postId, issuerId: GetCurrentUserId());
            return Ok();
        }
    }
}