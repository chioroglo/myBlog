using Microsoft.AspNetCore.Http;

namespace DAL.Extensions
{
    public static class IFormFileExtensions
    {
        public static async Task CopyInPathOnDiskAsync(this IFormFile file, string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }
    }
}