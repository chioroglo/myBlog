using API.Controllers.Base;
using AutoMapper;
using Common.Dto.Comment;
using Common.Dto.Paging.OffsetPaging;
using Common.Models;
using Domain;
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

        [HttpGet("{commentId:int}")]
        public async Task<CommentModel> GetByIdAsync(int commentId, CancellationToken cancellationToken)
        {
            var result = await _commentsService.GetByIdAsync(commentId,cancellationToken);

            CommentModel commentModel = _mapper.Map<CommentModel>(result);
            
            return commentModel;
        }

        [HttpGet("[action]/{postId:int}")]
        public async Task<IEnumerable<CommentModel>> GetByPostIdAsync(int postId, CancellationToken cancellationToken)
        {
            IEnumerable<Comment> comments = await _commentsService.GetCommentsByPostId(postId,cancellationToken);

            return comments.Select(e => _mapper.Map<CommentModel>(e)).ToList();
        }


        [HttpPost]
        public async Task<CommentModel> CreateCommentAsync([FromBody] CommentDto request, CancellationToken cancellationToken)
        {
            var commentEntity = _mapper.Map<Comment>(request);

            commentEntity.UserId = GetCurrentUserId();

            await _commentsService.Add(commentEntity,cancellationToken);

            commentEntity.RegistrationDate = DateTime.UtcNow;

            return _mapper.Map<CommentModel>(commentEntity);
        }

        
        [HttpPut("{commentId:int}")]
        public async Task<CommentModel> EditCommentAsync(int commentId, [FromBody] CommentDto updateRequest, CancellationToken cancellationToken)
        {
            var commentEntity = _mapper.Map<Comment>(updateRequest);

            commentEntity.Id = commentId;
            commentEntity.UserId = GetCurrentUserId();

            await _commentsService.UpdateAsync(commentEntity,cancellationToken);

            return _mapper.Map<CommentModel>(commentEntity);
        }

        [HttpDelete("{commentId:int}")]
        public async Task DeleteCommentAsync(int commentId, CancellationToken cancellationToken)
        {
            var comment = await _commentsService.GetByIdAsync(commentId,cancellationToken);

            var currentUserId = GetCurrentUserId();

            await _commentsService.RemoveAsync(commentId,issuerId: currentUserId,cancellationToken);
        }

        [HttpPost("paginated-search")]
        public async Task<OffsetPagedResult<CommentModel>> GetPageWithUserAsync([FromBody] OffsetPagedRequest query, CancellationToken cancellationToken)
        {
            var response = await _commentsService.GetOffsetPageAsync(query,cancellationToken,e => e.Post,e => e.User);

            return new OffsetPagedResult<CommentModel>()
            {
                PageIndex = response.PageIndex,
                PageSize = response.PageSize,
                Total = response.Total,
                Items = response.Items.Select(e => _mapper.Map<CommentModel>(e)).ToList()
            };
            
        }
    }
}