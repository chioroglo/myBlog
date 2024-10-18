using API.Middlewares;

namespace API.Extensions
{
    public static class MiddlewareExtensions
    {

        public static IApplicationBuilder UseDatabaseTransactions(this IApplicationBuilder app)
        {
            return app.UseMiddleware<DbTransactionMiddleware>();
        }
    }
}