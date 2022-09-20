using DAL.Repositories.Abstract;
using Domain;
using Domain.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Service.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Service
{
    public class AvatarService : IFileHandlingService
    {
        private readonly IAvatarRepository _avatarRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _uploadSubDirectoryInWwwRoot;

        public AvatarService(IAvatarRepository avatarRepository, IWebHostEnvironment webHostEnvironment)
        {
            _avatarRepository = avatarRepository;
            _webHostEnvironment = webHostEnvironment;
            _uploadSubDirectoryInWwwRoot = "data";
        }

        public async Task<byte[]> Add(IFormFile image, int userId)
        {

            if (image.Length == 0)
            {
                throw new ValidationException("Empty image introduced");
            }
                
            if (await _avatarRepository.GetByUserIdAsync(userId) != null)
            {
                throw new ValidationException("This user already has an avatar!");
            }

            string filePath = ComposePathForNewAvatar(image, userId);
            await image.CopyInfPathOnDiskAsync(filePath);

            var entity = new Avatar()
            {
                UserId = userId,
                Url = filePath
            };

            _avatarRepository.Add(entity);
            await _avatarRepository.SaveChangesAsync();

            return await image.ToByteArrayAsync();
        }

        private string ComposePathForNewAvatar(IFormFile image, int userId)
        {
            string directoryPath = Path.Combine(_webHostEnvironment.WebRootPath, _uploadSubDirectoryInWwwRoot, nameof(Avatar));

            string fileName = userId + Path.GetExtension(image.FileName);

            string filePath = Path.Combine(directoryPath, fileName);

            return filePath;
        }

        public async Task<byte[]> GetByUserIdAsync(int userId)
        {
            var avatarInfo = await GetAvatarInfoThrowValidationExceptionIfNotFound(userId);
            var path = avatarInfo.Url;

            return await RetrieveAvatarFromDiskAsync(path);
        }
        
        private async Task<byte[]> RetrieveAvatarFromDiskAsync(string path)
        {
            byte[] byteImage;

            using (var fileStream = new FileStream(path, FileMode.Open))
            {
                using (var memoryStream = new MemoryStream())
                {
                    await fileStream.CopyToAsync(memoryStream);

                    byteImage = memoryStream.ToArray();

                }
            }
            return byteImage;
        }

        public async Task Remove(int issuerId)
        {
            var avatarInfo = await GetAvatarInfoThrowValidationExceptionIfNotFound(issuerId);
            var path = avatarInfo.Url;

            RemoveAvatarOnDisk(path);

            await _avatarRepository.RemoveAsync(avatarInfo.Id);
            await _avatarRepository.SaveChangesAsync();
        }

        public async Task<byte[]> Update(IFormFile image, int userId)
        {
            var avatarInfo = await GetAvatarInfoThrowValidationExceptionIfNotFound(userId);
            var path = avatarInfo.Url;

            RemoveAvatarOnDisk(path);
            await image.CopyInfPathOnDiskAsync(path);

            _avatarRepository.Update(avatarInfo);
            await _avatarRepository.SaveChangesAsync();
            
            return await image.ToByteArrayAsync();
        }

        private async Task<Avatar> GetAvatarInfoThrowValidationExceptionIfNotFound(int userId)
        {
            var avatarInfo = await _avatarRepository.GetByUserIdAsync(userId);

            if (avatarInfo == null)
            {
                throw new ValidationException($"{nameof(Avatar)} of {nameof(User)}ID: {userId} was not found");
            }

            return avatarInfo;
        }

        private void RemoveAvatarOnDisk(string path)
        {
            using (new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None, 4096, FileOptions.DeleteOnClose))
            {

            }
        }
    }
}