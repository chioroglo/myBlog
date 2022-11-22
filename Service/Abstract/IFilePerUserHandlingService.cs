using Microsoft.AspNetCore.Http;

namespace Service.Abstract
{
    public interface IFilePerUserHandlingService
    {
        Task<string> GetFileNameByUserIdAsync(int userId, CancellationToken cancellationToken);

        Task<string> AddAsyncAndRetrieveFileName(IFormFile file, int userId, CancellationToken cancellationToken);

        Task RemoveAsync(int issuerId, CancellationToken cancellationToken);

        Task<string> UpdateFileAsyncAndRetrieveFileName(IFormFile file, int userId,
            CancellationToken cancellationToken);
    }
}