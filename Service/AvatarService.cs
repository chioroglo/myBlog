using DAL.Repositories.Abstract;
using Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Service.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Service
{
    public class AvatarService : IAvatarService
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

            if (await _avatarRepository.GetByUserId(userId) != null)
            {
                throw new ValidationException("This user already has an avatar!");
            }

            string directoryPath = Path.Combine(_webHostEnvironment.WebRootPath, _uploadSubDirectoryInWwwRoot, $"{nameof(Avatar)}");

            string fileName = userId + Path.GetExtension(image.FileName);

            string filePath = Path.Combine(directoryPath, fileName);


            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            byte[] imgByteArray;
            using (var memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream);

                imgByteArray = memoryStream.ToArray();
            };

            var entity = new Avatar()
            {
                UserId = userId,
                Url = filePath
            };

            await _avatarRepository.AddAsync(entity);
            await _avatarRepository.SaveChangesAsync();

            return imgByteArray;
        }

        public async Task<byte[]> GetByUserIdAsync(int userId)
        {
            var avatarInfo = await _avatarRepository.GetByUserId(userId);

            if (avatarInfo == null)
            {
                throw new ValidationException($"{nameof(Avatar)} of {nameof(User)}ID: {userId} was not found");
            }

            var path = avatarInfo.Url;

            using (var fileStream = new FileStream(path,FileMode.Open))
            {
                using (var memoryStream = new MemoryStream())
                {
                    await fileStream.CopyToAsync(memoryStream);

                    byte[] byteImage = memoryStream.ToArray();

                    return byteImage;
                }
            }
        }

        public async Task Remove(int issuerId)
        {
            var avatarInfo = await _avatarRepository.GetByUserId(issuerId);

            if (avatarInfo == null)
            {
                throw new ValidationException($"{nameof(Avatar)} of {nameof(User)}ID: {issuerId} was not found");
            }

            var path = avatarInfo.Url;

            RemoveFile(avatarInfo.Url);
            
            await _avatarRepository.RemoveAsync(avatarInfo.Id);

            await _avatarRepository.SaveChangesAsync();
        }

        public async Task<byte[]> Update(IFormFile image, int userId)
        {
            var avatarInfo = await _avatarRepository.GetByUserId(userId);

            if (avatarInfo == null)
            {
                throw new ValidationException($"{nameof(Avatar)} of {nameof(User)}ID: {userId} was not found");
            }

            var path = avatarInfo.Url;

            if (File.Exists(path))
            {
                RemoveFile(path);
            }


            byte[] byteImageArray;

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(fileStream);

                    await image.CopyToAsync(memoryStream);

                    byteImageArray = memoryStream.ToArray();
                }

            }

            await _avatarRepository.SaveChangesAsync();
            
            return byteImageArray;
        }

        private void RemoveFile(string path)
        {
            using (new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None, 4096, FileOptions.DeleteOnClose))
            {

            }
        }
    }
}