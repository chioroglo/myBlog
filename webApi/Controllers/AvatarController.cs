using API.Controllers.Base;
using API.Filters;
using Common.Dto.Avatar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;

namespace API.Controllers
{
    [Route("api/avatars")]
    public class AvatarController : AppBaseController
    {
        private readonly IFilePerUserHandlingService _avatarService;
        private readonly UriBuilder _uriBuilder;

        public AvatarController(IFilePerUserHandlingService avatarService, IUserService userService) : base(userService)
        {
            _avatarService = avatarService;
            _uriBuilder = new UriBuilder();
        }

        [AllowAnonymous]
        [HttpGet("{userId:int}")]
        public async Task<string> GetAvatarLinkByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            SupplyWithApplicationUrl(_uriBuilder);

            var fileName = await _avatarService.GetFileNameByUserIdAsync(userId, cancellationToken);

            _uriBuilder.Path = fileName;

            return _uriBuilder.ToString();
        }

        [HttpPost]
        [UpdatesUserActivity]
        public async Task<string> UploadAvatarAndReturnLinkAsync([FromForm] AvatarDto request,
            CancellationToken cancellationToken)
        {
            SupplyWithApplicationUrl(_uriBuilder);
            request.UserId = CurrentUserId;

            var fileName = await _avatarService.AddAsyncAndRetrieveFileName(request.Image, request.UserId, cancellationToken);
            _uriBuilder.Path = fileName;

            return _uriBuilder.ToString();
        }

        [HttpPut]
        [UpdatesUserActivity]
        public async Task<string> UpdateAvatarAsync([FromForm] AvatarDto request, CancellationToken cancellationToken)
        {
            SupplyWithApplicationUrl(_uriBuilder);
            request.UserId = CurrentUserId;

            var fileName =
                await _avatarService.UpdateFileAsyncAndRetrieveFileName(request.Image, request.UserId,
                    cancellationToken);
            _uriBuilder.Path = fileName;

            return _uriBuilder.ToString();
        }

        [HttpDelete]
        [UpdatesUserActivity]
        public async Task RemoveAvatarAsync(CancellationToken cancellationToken)
        {
            await _avatarService.RemoveAsync(CurrentUserId, cancellationToken);
        }

        [NonAction]
        // Do this as property
        private void SupplyWithApplicationUrl(UriBuilder uriBuilder)
        {
            uriBuilder.Scheme = Request.Scheme;
            uriBuilder.Host = Request.Host.Host;
            uriBuilder.Port = Request.Host.Port ?? 80;
        }
    }
}