using API.Controllers.Base;
using AutoMapper;
using Domain;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;

namespace API.Controllers
{
    [Route("api/comments")]
    public class CommentsController : AppBaseController
    {
        private readonly ICommentService _commentsService;
        private readonly IMapper _mapper;

        public CommentsController(ICommentService commentsService, IMapper mapper)
        {
            _commentsService = commentsService;
            _mapper = mapper;
        }

        [Route("{id:int}")]
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _commentsService.GetById(id);

            CommentModel commentModel = _mapper.Map<CommentModel>(result);
            
            return Ok(commentModel);
        }

        [Route("[action]/{postId:int}")]
        [HttpGet]
        public async Task<IActionResult> GetByPostId(int postId)
        {
            IEnumerable<Comment> comments = await _commentsService.GetCommentsByPostId(postId);

            return Ok(comments.Select(e => _mapper.Map<CommentModel>(e)).ToList());      
        }
    }
}
