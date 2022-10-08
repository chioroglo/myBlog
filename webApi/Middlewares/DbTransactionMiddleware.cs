using DAL;
using Microsoft.EntityFrameworkCore;

namespace API.Middlewares
{
    public class DbTransactionMiddleware
    {
        private readonly RequestDelegate _next;

        public DbTransactionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, BlogDbContext dbContext)
        {
            if (httpContext.Request.Method == HttpMethod.Get.Method)
            {
                await _next(httpContext);
                return;
            }

            using (var transaction = await dbContext.Database.BeginTransactionAsync(System.Data.IsolationLevel.RepeatableRead))
            {
                Console.WriteLine("TRANSACTION HAS BEGUN");
                await _next(httpContext);

                await dbContext.SaveChangesAsync();
                //Commit transaction if all commands succeed, transaction will auto-rollback when disposed if either commands fails
                await transaction.CommitAsync();
            }

        }
    }
}
