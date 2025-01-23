using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.API.Controllers.Base;
using MyBlog.API.Filters;
using MyBlog.Common.Dto.Avatar;
using MyBlog.Service.Abstract;

namespace MyBlog.API.Controllers
{
    [Route("api/avatars")]
    public class AvatarController : AppBaseController
    {
        private readonly IAvatarService _avatarService;

        public AvatarController(IAvatarService avatarService) 
        {
            _avatarService = avatarService;
        }

        [AllowAnonymous]
        [HttpGet("{userId:int:min(0)}")]
        public async Task<IActionResult> GetAvatarLinkByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            var url = await _avatarService.GetAvatarUrlAsync(userId, cancellationToken);

            return string.IsNullOrEmpty(url) ? NotFound() : Ok(url);
        }

        [HttpPost]
        [UpdatesUserActivity]
        public async Task<string?> UploadAvatarAndReturnLinkAsync([FromForm] AvatarDto request, CancellationToken cancellationToken)
        {
            var uri = await _avatarService.AddAndGetUrlAsync(request.Image, CurrentUserId, cancellationToken);
            return uri;
        }

        [HttpDelete]
        [UpdatesUserActivity]
        public async Task<IActionResult> RemoveAvatarAsync(CancellationToken cancellationToken)
        {
            await _avatarService.RemoveAsync(CurrentUserId, cancellationToken);
            return NoContent();
        }
    }
}