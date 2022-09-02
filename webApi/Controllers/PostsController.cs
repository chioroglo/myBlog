using AutoMapper;
using Domain;
using Domain.Dto.Post;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBlog.Service.Auth;
using Service.Abstract;
using System.Security.Claims;

namespace webApi.Controllers
{
    [Route("api/posts")]
    public class PostsController : AppBaseController
    {
        private readonly IPostsService _postsService;
        private readonly IMapper _mapper;

        public PostsController(IPostsService postsService,IMapper mapper)
        {
            _postsService = postsService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostModel>>> Get()
        {
            var posts = await _postsService.GetAll();
            return posts.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostModel>> Get(int id)
        {
            PostModel post = await Task.FromResult( _postsService.GetById(id) ).Result;
            return post == null ?
                NotFound($"post [POST ID: {id} was not found]")
                :
                post;
            
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(PostDto postContent)
        {
            try
            {
                PostModel postRequest = _mapper.Map<PostModel>(postContent);

                postRequest.AuthorId = Convert.ToInt32(HttpContext.User.FindFirstValue(TokenClaimNames.Id));

                await _postsService.Add(postRequest);


                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine($"[api/posts/CreatePost] {e.Message}");
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PostDto>> Put(int id,PostDto post)
        {
            PostModel updateRequest = _mapper.Map<PostModel>(post);

            try
            {
                await _postsService.Update(updateRequest);
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (PostDoesNotExist(id))
                {
                    return NotFound($"Post [POST ID: {id}] does not exist");
                }
                else
                {
                    throw;
                }
            }

            return Ok(post);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var post = await _postsService.GetById(id);
                var currentUserId = Convert.ToInt32(User.FindFirstValue(TokenClaimNames.Id));

                if (post.AuthorId == currentUserId)
                {
                    await _postsService.Remove(id);
                    return Ok($"successfully deleted post {id}");
                }
                else
                {
                    return BadRequest($"This post[POST ID: {id}] belongs to [USER ID:{post.AuthorId}]. Request came from [USER ID:{currentUserId}]");
                }
            }
            catch(NullReferenceException e)
            {
                if (PostDoesNotExist(id))
                {
                    return BadRequest($"Post [POST ID : {id}] does not exist");
                }

                return BadRequest(e.Message);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [NonAction]
        private bool PostDoesNotExist(int id)
        {
            return _postsService.GetById(id) != null;
        }
    }
}
