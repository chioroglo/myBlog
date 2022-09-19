using Microsoft.AspNetCore.Http;

namespace Domain.Extensions
{
    public static class IFormFileExtensions
    {
        public static async Task<byte[]> ToByteArrayAsync(this IFormFile file)
        {
            byte[] imgByteArray;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);

                imgByteArray = memoryStream.ToArray();
            };

            return imgByteArray;
        }

        public static async Task CopyInfPathOnDiskAsync(this IFormFile file,string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }
    }
}
