using API.Controllers.Base;
using Domain.Dto.Avatar;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;

namespace API.Controllers
{
    [Route("api/avatar")]
    public class AvatarController : AppBaseController
    {
        private readonly IFileHandlingService _avatarService;

        public AvatarController(IFileHandlingService avatarService)
        {
            _avatarService = avatarService;
        }

        [HttpPost]
        public async Task<FileContentResult> UploadAvatar([FromForm] AvatarDto request)
        {
            request.UserId = GetCurrentUserId();
            var byteImage = await _avatarService.Add(request.Image, request.UserId);

            return File(byteImage,contentType:"image/jpeg");
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
            var byteImage = await _avatarService.Update(request.Image, request.UserId);

            return File(byteImage,"image/jpeg");
        }

        [HttpDelete]
        public async Task RemoveAvatarOfCurrentUser()
        {
            await _avatarService.Remove(GetCurrentUserId());
        }
    }

}