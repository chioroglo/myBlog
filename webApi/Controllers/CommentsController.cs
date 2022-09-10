using API.Controllers.Base;
using AutoMapper;
using Domain;
using Domain.Dto.Comment;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet]
        public async Task<IEnumerable<CommentModel>> GetAll()
        {
            var result = await _commentsService.GetAll();

            return result.Select(c => _mapper.Map<CommentModel>(c));
        }

        [Route("{commentId:int}")]
        [HttpGet]
        public async Task<CommentModel> GetById(int commentId)
        {
            var result = await _commentsService.GetById(commentId);

            CommentModel commentModel = _mapper.Map<CommentModel>(result);
            
            return commentModel;
        }

        [Route("[action]/{postId:int}")]
        [HttpGet]
        public async Task<IEnumerable<CommentModel>> GetByPostId(int postId)
        {
            IEnumerable<Comment> comments = await _commentsService.GetCommentsByPostId(postId);

            return comments.Select(e => _mapper.Map<CommentModel>(e)).ToList();
        }


        [HttpPost]
        public async Task CreateComment([FromBody] CommentDto request)
        {
            var commentEntity = _mapper.Map<Comment>(request);

            commentEntity.UserId = GetCurrentUserId();

            await _commentsService.Add(commentEntity);
        }

        
        [HttpPut("{commentId:int}")]
        public async Task EditComment(int commentId, [FromBody] CommentDto updateRequest)
        {
            var commentEntity = _mapper.Map<Comment>(updateRequest);

            commentEntity.Id = commentId;
            commentEntity.UserId = GetCurrentUserId();

            try
            {
                await _commentsService.Update(commentEntity);
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpDelete("{commentId:int}")]
        public async Task DeleteComment(int commentId)
        {
            var comment = await _commentsService.GetById(commentId);

            var currentUserId = GetCurrentUserId();

            await _commentsService.Remove(commentId,issuerId: currentUserId);
        }
    }
}