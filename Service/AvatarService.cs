using Common.Exceptions;
using DAL.Extensions;
using DAL.Repositories.Abstract;
using Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Service.Abstract;
using System.Drawing;
using static Common.Validation.EntityConfigurationConstants;

namespace Service
{
    public class AvatarService : IFilePerUserHandlingService
    {
        private readonly IAvatarRepository _avatarRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _uploadSubDirectoryInWwwRoot;
        private readonly string _absoluteDirectoryPath;
        private readonly string _relativePathInWwwRoot;

        public AvatarService(IAvatarRepository avatarRepository, IWebHostEnvironment webHostEnvironment)
        {
            _avatarRepository = avatarRepository;
            _webHostEnvironment = webHostEnvironment;
            _uploadSubDirectoryInWwwRoot = "data";
            _absoluteDirectoryPath = Path.Combine(_webHostEnvironment.WebRootPath, _uploadSubDirectoryInWwwRoot, nameof(Avatar));
            _relativePathInWwwRoot = Path.Combine(_uploadSubDirectoryInWwwRoot, nameof(Avatar));
        }

        public async Task<string> AddAsyncAndRetrieveFileName(IFormFile image, int userId,CancellationToken cancellationToken)
        {
                
            if (await _avatarRepository.GetByUserIdAsync(userId,cancellationToken) != null)
            {
                throw new ValidationException("This user already has an avatar uploaded!");
            }

            ValidateImageSize(image);

            if (!Directory.Exists(_absoluteDirectoryPath))
            {
                Directory.CreateDirectory(_absoluteDirectoryPath);
            }

            string fileName = ComposeFileNameWithExtension(image, userId);
            string absolutePath = ComposeAbsolutePath(fileName);
            
            await image.CopyInfPathOnDiskAsync(absolutePath);

            var entity = new Avatar()
            {
                UserId = userId,
                Url = fileName
            };

            await _avatarRepository.AddAsync(entity,cancellationToken);

            return Path.Combine(_relativePathInWwwRoot,fileName);
        }

        public async Task<string> GetFileNameByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            var avatarInfo = await GetAvatarInfoThrowValidationExceptionIfNotFoundAsync(userId, cancellationToken);
            
            var fileName = avatarInfo.Url;

            return Path.Combine(_relativePathInWwwRoot, fileName);
        }
        
        public async Task<string> UpdateFileAsyncAndRetrieveFileName(IFormFile image, int userId, CancellationToken cancellationToken)
        {
            ValidateImageSize(image);

            var avatarInfo = await GetAvatarInfoThrowValidationExceptionIfNotFoundAsync(userId, cancellationToken);
            var path = ComposeAbsolutePath(avatarInfo.Url);
            var fileName = ComposeFileNameWithExtension(image, userId);

            RemoveAvatarOnDisk(path);
            await image.CopyInfPathOnDiskAsync(path);


            avatarInfo.Url = path;
            _avatarRepository.Update(avatarInfo, cancellationToken);

            
            return Path.Combine(_relativePathInWwwRoot, fileName);
        }
        
        public async Task RemoveAsync(int issuerId, CancellationToken cancellationToken)
        {
            var avatarInfo = await GetAvatarInfoThrowValidationExceptionIfNotFoundAsync(issuerId,cancellationToken);
            var path = ComposeAbsolutePath(avatarInfo.Url);

            RemoveAvatarOnDisk(path);

            await _avatarRepository.RemoveAsync(avatarInfo.Id,cancellationToken);
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
        
        private async Task<Avatar> GetAvatarInfoThrowValidationExceptionIfNotFoundAsync(int userId, CancellationToken cancellationToken)
        {
            var avatarInfo = await _avatarRepository.GetByUserIdAsync(userId,cancellationToken);

            if (avatarInfo == null)
            {
                throw new ValidationException($"{nameof(Avatar)} of {nameof(User)}ID: {userId} was not found");
            }

            return avatarInfo;
        }

        private string ComposeFileNameWithExtension(IFormFile image, int userId)
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
                if (image.Width > MaxAvatarWidthPx
                    || image.Width < MinAvatarWidthPx
                    || image.Height > MaxAvatarHeightPx
                    || image.Width < MinAvatarHeightPx)
                {
                    throw new ValidationException($"Invalid image size (W:{image.Width} H:{image.Height}) introduced");
                }
            }
        }


    }
}