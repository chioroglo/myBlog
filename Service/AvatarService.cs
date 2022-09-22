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
        private readonly string _absoluteDirectoryPath;

        public AvatarService(IAvatarRepository avatarRepository, IWebHostEnvironment webHostEnvironment)
        {
            _avatarRepository = avatarRepository;
            _webHostEnvironment = webHostEnvironment;
            _uploadSubDirectoryInWwwRoot = "data";
            _absoluteDirectoryPath = Path.Combine(_webHostEnvironment.WebRootPath, _uploadSubDirectoryInWwwRoot, nameof(Avatar));
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

            if(!Directory.Exists(_absoluteDirectoryPath))
            {
                Directory.CreateDirectory(_absoluteDirectoryPath);
            }

            string relativePath = ComposeRelativePath(image, userId);
            string absolutePath = ComposeAbsolutePath(relativePath);
            
            await image.CopyInfPathOnDiskAsync(absolutePath);

            var entity = new Avatar()
            {
                UserId = userId,
                Url = relativePath
            };

            _avatarRepository.Add(entity);
            await _avatarRepository.SaveChangesAsync();

            return await image.ToByteArrayAsync();
        }

        public async Task<byte[]> GetByUserIdAsync(int userId)
        {
            var avatarInfo = await GetAvatarInfoThrowValidationExceptionIfNotFound(userId);
            var path = avatarInfo.Url;

            return await RetrieveAvatarFromDiskAsync(path);
        }

        
        public async Task Remove(int issuerId)
        {
            var avatarInfo = await GetAvatarInfoThrowValidationExceptionIfNotFound(issuerId);
            var path = ComposeAbsolutePath(avatarInfo.Url);

            RemoveAvatarOnDisk(path);

            await _avatarRepository.RemoveAsync(avatarInfo.Id);
            await _avatarRepository.SaveChangesAsync();
        }

        public async Task<byte[]> Update(IFormFile image, int userId)
        {
            var avatarInfo = await GetAvatarInfoThrowValidationExceptionIfNotFound(userId);
            var path = ComposeAbsolutePath(avatarInfo.Url);

            RemoveAvatarOnDisk(path);
            await image.CopyInfPathOnDiskAsync(path);

            _avatarRepository.Update(avatarInfo);
            await _avatarRepository.SaveChangesAsync();
            
            return await image.ToByteArrayAsync();
        }
        

        private async Task<byte[]> RetrieveAvatarFromDiskAsync(string relativePath)
        {
            string path = ComposeAbsolutePath(relativePath);

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
        
        private async Task<Avatar> GetAvatarInfoThrowValidationExceptionIfNotFound(int userId)
        {
            var avatarInfo = await _avatarRepository.GetByUserIdAsync(userId);

            if (avatarInfo == null)
            {
                throw new ValidationException($"{nameof(Avatar)} of {nameof(User)}ID: {userId} was not found");
            }

            return avatarInfo;
        }

        private string ComposeRelativePath(IFormFile image, int userId)
        {
            string relativeFilePath = userId + Path.GetExtension(image.FileName);

            return relativeFilePath;
        }

        private string ComposeAbsolutePath(string relativeUrl)
        {
            return Path.Combine(_absoluteDirectoryPath, relativeUrl);
        }
        
        private void RemoveAvatarOnDisk(string path)
        {
            using (new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None, 4096, FileOptions.DeleteOnClose))
            {

            }
        }
    }
}