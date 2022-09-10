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
        public async Task<IActionResult> CreatePost(PostDto postContent)
        {
            try
            {
                PostModel postRequest = _mapper.Map<PostModel>(postContent);

                postRequest.AuthorId = GetCurrentUserId();

                await _postsService.Add(postRequest);


                return Ok($"post was successfully created!");
            }
            catch (Exception e)
            {
                Console.WriteLine($"[api/posts/CreatePost] {e.Message}");
                return BadRequest();
            }
        }

        [HttpPut("{postId:int}")]
        public async Task<IActionResult> UpdatePost(int postId,PostDto post)
        {

            PostModel updateRequest = _mapper.Map<PostModel>(post);

            updateRequest.Id = postId;
            updateRequest.AuthorId = GetCurrentUserId();

            bool updateIsSuccessfull = false; 
            try
            {
                updateIsSuccessfull = await _postsService.Update(updateRequest);
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (PostDoesNotExist(postId))
                {
                    return NotFound($"Post [POST ID: {postId}] does not exist");
                }
                else
                {
                    throw;
                }
            }

            var updatedPost = await _postsService.GetById(postId);

            return updateIsSuccessfull ? Ok(updatedPost) : BadRequest();
        }

        [HttpDelete("{postId:int}")]
        public async Task<ActionResult> Delete(int postId)
        {
            try
            {
                var post = await _postsService.GetById(postId);
                var currentUserId = GetCurrentUserId();

                if (post.AuthorId == currentUserId)
                {
                    await _postsService.Remove(postId);
                    return Ok($"successfully deleted post {postId}");
                }
                else
                {
                    return BadRequest($"This post[POST ID: {postId}] belongs to [USER ID:{post.AuthorId}]. Request came from [USER ID:{currentUserId}]");
                }
            }
            catch(NullReferenceException e)
            {
                if (PostDoesNotExist(postId))
                {
                    return BadRequest($"Post [POST ID : {postId}] does not exist");
                }

                return BadRequest(e.Message);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [NonAction]
        private bool PostDoesNotExist(int postId)
        {
            return _postsService.GetById(postId) != null;
        }
    }
}