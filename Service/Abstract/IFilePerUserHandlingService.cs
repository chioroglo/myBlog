using Microsoft.AspNetCore.Http;

namespace Service.Abstract
{
    public interface IFilePerUserHandlingService
    {
        Task<byte[]> GetByUserIdAsync(int userId, CancellationToken cancellationToken);

        Task<byte[]> AddAsync(IFormFile file, int userId, CancellationToken cancellationToken);

        Task RemoveAsync(int issuerId, CancellationToken cancellationToken);

        Task<byte[]> UpdateAsync(IFormFile file, int userId, CancellationToken cancellationToken);
    }
}
