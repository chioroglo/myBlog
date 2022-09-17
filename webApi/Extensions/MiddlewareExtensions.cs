using API.Middlewares;

namespace API.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app) => app.UseMiddleware<ExceptionHandlingMiddleware>();

        public static IApplicationBuilder UseDatabaseTransactions(this IApplicationBuilder app) => app.UseMiddleware<DbTransactionMiddleware>();
    }
}
