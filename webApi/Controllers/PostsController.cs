using AutoMapper;
using Domain;
using Domain.Dto.Post;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Abstract;
using System.Net;
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
        public async Task<ActionResult<IEnumerable<PostEntity>>> Get()
        {
            var posts = await _postsService.GetAll();
            return posts.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostEntity>> Get(int id)
        {
            PostEntity post = await Task.FromResult( _postsService.GetById(id) ).Result;
            return post == null ? NotFound() : post;
            
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(PostDto postContent)
        {
            try
            {
                postContent.AuthorId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var postEntity = _mapper.Map<PostDto, PostEntity>(postContent);

                await _postsService.Add(postEntity);


                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine($"[api/posts/CreatePost] {e.Message}");
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PostEntity>> Put(int id,PostEntity post)
        {
            if (id != post.Id)
            {
                return BadRequest();
            }

            try
            {
                await _postsService.Update(post);
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (PostDoesNotExist(post.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return await Task.FromResult(post);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var post = await _postsService.GetById(id);
                var currentUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                if (post.AuthorId == currentUserId)
                {
                    await _postsService.Remove(id);
                    return Ok($"successfully deleted post {id}");
                }
                else
                {
                    return BadRequest($"This post[POST ID: {id}] does not belong to this user [USER ID: {currentUserId}]");
                }
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
