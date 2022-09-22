using DAL.Repositories.Abstract;
using Domain;
using Domain.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Service.Abstract;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using static Domain.Validation.EntityConfigurationConstants;

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

        public async Task<byte[]> Add(IFormFile image, int userId,CancellationToken cancellationToken)
        {
                
            if (await _avatarRepository.GetByUserIdAsync(userId,cancellationToken) != null)
            {
                throw new ValidationException("This user already has an avatar!");
            }

            ValidateImageSize(image);

            if (!Directory.Exists(_absoluteDirectoryPath))
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

            await _avatarRepository.AddAsync(entity,cancellationToken);
            await _avatarRepository.SaveChangesAsync(cancellationToken);

            return await image.ToByteArrayAsync();
        }

        public async Task<byte[]> GetByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            var avatarInfo = await GetAvatarInfoThrowValidationExceptionIfNotFound(userId,cancellationToken);
            var path = avatarInfo.Url;

            return await RetrieveAvatarFromDiskAsync(path,cancellationToken);
        }

        
        public async Task RemoveAsync(int issuerId, CancellationToken cancellationToken)
        {
            var avatarInfo = await GetAvatarInfoThrowValidationExceptionIfNotFound(issuerId,cancellationToken);
            var path = ComposeAbsolutePath(avatarInfo.Url);

            RemoveAvatarOnDisk(path);

            await _avatarRepository.RemoveAsync(avatarInfo.Id,cancellationToken);
            await _avatarRepository.SaveChangesAsync(cancellationToken);
        }

        public async Task<byte[]> UpdateAsync(IFormFile image, int userId, CancellationToken cancellationToken)
        {
            ValidateImageSize(image);

            var avatarInfo = await GetAvatarInfoThrowValidationExceptionIfNotFound(userId,cancellationToken);
            var path = ComposeAbsolutePath(avatarInfo.Url);

            RemoveAvatarOnDisk(path);
            await image.CopyInfPathOnDiskAsync(path);

            _avatarRepository.Update(avatarInfo,cancellationToken);
            await _avatarRepository.SaveChangesAsync(cancellationToken);
            
            return await image.ToByteArrayAsync();
        }
        

        private async Task<byte[]> RetrieveAvatarFromDiskAsync(string relativePath, CancellationToken cancellationToken)
        {
            string path = ComposeAbsolutePath(relativePath);

            byte[] byteImage;

            using (var fileStream = new FileStream(path, FileMode.Open))
            {
                using (var memoryStream = new MemoryStream())
                {
                    await fileStream.CopyToAsync(memoryStream,cancellationToken);

                    byteImage = memoryStream.ToArray();
                }
            }
            return byteImage;
        }
        
        private async Task<Avatar> GetAvatarInfoThrowValidationExceptionIfNotFound(int userId, CancellationToken cancellationToken)
        {
            var avatarInfo = await _avatarRepository.GetByUserIdAsync(userId,cancellationToken);

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

        private void ValidateImageSize(IFormFile file)
        {
            using (var image = Image.FromStream(file.OpenReadStream()))
            {
                if (image.Width > MAX_AVATAR_WIDTH_PX
                    || image.Width < MIN_AVATAR_WIDTH_PX
                    || image.Height > MAX_AVATAR_HEIGHT_PX
                    || image.Width < MIN_AVATAR_HEIGHT_PX)
                {
                    throw new ValidationException($"Invalid image size introduced W:{image.Width} H:{image.Height}");
                }
            }
        }
    }
}