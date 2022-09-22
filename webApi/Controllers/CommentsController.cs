using API.Controllers.Base;
using AutoMapper;
using Domain;
using Domain.Dto.Comment;
using Domain.Models;
using Domain.Models.Pagination;
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

        [HttpGet]
        public async Task<IEnumerable<CommentModel>> GetAll(CancellationToken cancellationToken)
        {
            var result = await _commentsService.GetAll(cancellationToken);

            return result.Select(c => _mapper.Map<CommentModel>(c));
        }

        [Route("{commentId:int}")]
        [HttpGet]
        public async Task<CommentModel> GetById(int commentId, CancellationToken cancellationToken)
        {
            var result = await _commentsService.GetById(commentId,cancellationToken);

            CommentModel commentModel = _mapper.Map<CommentModel>(result);
            
            return commentModel;
        }

        [Route("[action]/{postId:int}")]
        [HttpGet]
        public async Task<IEnumerable<CommentModel>> GetByPostId(int postId, CancellationToken cancellationToken)
        {
            IEnumerable<Comment> comments = await _commentsService.GetCommentsByPostId(postId,cancellationToken);

            return comments.Select(e => _mapper.Map<CommentModel>(e)).ToList();
        }


        [HttpPost]
        public async Task<CommentModel> CreateComment([FromBody] CommentDto request, CancellationToken cancellationToken)
        {
            var commentEntity = _mapper.Map<Comment>(request);

            commentEntity.UserId = GetCurrentUserId();

            await _commentsService.Add(commentEntity,cancellationToken);

            commentEntity.RegistrationDate = DateTime.UtcNow;

            return _mapper.Map<CommentModel>(commentEntity);
        }

        
        [HttpPut("{commentId:int}")]
        public async Task<CommentModel> EditComment(int commentId, [FromBody] CommentDto updateRequest, CancellationToken cancellationToken)
        {
            var commentEntity = _mapper.Map<Comment>(updateRequest);

            commentEntity.Id = commentId;
            commentEntity.UserId = GetCurrentUserId();

            await _commentsService.Update(commentEntity,cancellationToken);

            return _mapper.Map<CommentModel>(commentEntity);
        }

        [HttpDelete("{commentId:int}")]
        public async Task DeleteComment(int commentId, CancellationToken cancellationToken)
        {
            var comment = await _commentsService.GetById(commentId,cancellationToken);

            var currentUserId = GetCurrentUserId();

            await _commentsService.Remove(commentId,issuerId: currentUserId,cancellationToken);
        }

        [HttpPost("paginated-search")]
        public async Task<PaginatedResult<CommentModel>> GetPage(PagedRequest query, CancellationToken cancellationToken)
        {
            var response = await _commentsService.GetPage(query,cancellationToken);

            return new PaginatedResult<CommentModel>()
            {
                PageIndex = response.PageIndex,
                PageSize = response.PageSize,
                Total = response.Total,
                Items = response.Items.Select(e => _mapper.Map<CommentModel>(e)).ToList()
            };
            
        }
    }
}