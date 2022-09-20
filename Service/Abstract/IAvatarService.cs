using Microsoft.AspNetCore.Http;

namespace Service.Abstract
{
    public interface IFileHandlingService
    {
        Task<byte[]> GetByUserIdAsync(int userId);

        Task<byte[]> Add(IFormFile file, int userId);

        Task Remove(int issuerId);

        Task<byte[]> Update(IFormFile file, int userId);
    }
}
