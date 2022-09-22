using Microsoft.AspNetCore.Http;

namespace Service.Abstract
{
    public interface IFileHandlingService
    {
        Task<byte[]> GetByUserIdAsync(int userId, CancellationToken cancellationToken);

        Task<byte[]> Add(IFormFile file, int userId, CancellationToken cancellationToken);

        Task RemoveAsync(int issuerId, CancellationToken cancellationToken);

        Task<byte[]> UpdateAsync(IFormFile file, int userId, CancellationToken cancellationToken);
    }
}
