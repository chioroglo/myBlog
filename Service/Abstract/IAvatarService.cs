using Microsoft.AspNetCore.Http;

namespace Service.Abstract
{
    public interface IFileHandlingService
    {
        Task<byte[]> GetByUserIdAsync(int userId);

        Task<byte[]> Add(IFormFile file, int userId);

        Task RemoveAsync(int issuerId);

        Task<byte[]> UpdateAsync(IFormFile file, int userId);
    }
}
