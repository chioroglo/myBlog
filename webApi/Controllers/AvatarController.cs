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

        [HttpGet("{userId:int}")]
        public async Task<FileContentResult> GetAvatarByUserId(int userId, CancellationToken cancellationToken)
        {
            var byteImage = await _avatarService.GetByUserIdAsync(userId,cancellationToken);

            return File(byteImage,contentType: "image/jpeg");
        }
        
        [HttpPost]
        public async Task<FileContentResult> UploadAvatar([FromForm] AvatarDto request, CancellationToken cancellationToken)
        {
            request.UserId = GetCurrentUserId();
            var byteImage = await _avatarService.Add(request.Image, request.UserId,cancellationToken);

            return File(byteImage,contentType:"image/jpeg");
        }
            

        [HttpPut]
        public async Task<FileContentResult> UpdateAvatar([FromForm] AvatarDto request, CancellationToken cancellationToken)
        {
            request.UserId = GetCurrentUserId();
            var byteImage = await _avatarService.UpdateAsync(request.Image, request.UserId,cancellationToken);

            return File(byteImage,"image/jpeg");
        }

        [HttpDelete]
        public async Task RemoveAvatar(CancellationToken cancellationToken)
        {
            await _avatarService.RemoveAsync(GetCurrentUserId(),cancellationToken);
        }
    }

}