using API.Controllers.Base;
using Domain.Dto.Avatar;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;

namespace API.Controllers
{

    [Route("api/avatar")]
    public class AvatarController : AppBaseController
    {
        private readonly IAvatarService _avatarService;

        public AvatarController(IAvatarService avatarService)
        {
            _avatarService = avatarService;
        }

        [HttpPost]
        public async Task<FileContentResult> UploadAvatar([FromForm] AvatarDto request)
        {
            request.UserId = GetCurrentUserId();

            return File(await _avatarService.Add(request.Image, request.UserId),contentType:"image/jpeg");
        }

        [Route("{userId:int}")]
        [HttpGet]
        public async Task<FileContentResult> GetAvatar(int userId)
        {
            var byteImage = await _avatarService.GetByUserIdAsync(userId);

            return File(byteImage,contentType: "image/jpeg");
        }

        [HttpPut()]
        public async Task<FileContentResult> UpdateAvatar([FromForm] AvatarDto request)
        {
            request.UserId = GetCurrentUserId();

            return File(await _avatarService.Update(request.Image, request.UserId),"image/jpeg");
        }

        [HttpDelete]
        public async Task RemoveAvatarOfCurrentUser()
        {
            await _avatarService.Remove(GetCurrentUserId());

        }
    }

}