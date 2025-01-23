using System.Security.Claims;
using MyBlog.Common;
using MyBlog.Common.Exceptions;
using MyBlog.Data.Repositories.Abstract;

namespace MyBlog.API.Middlewares;

public class BannedUserMiddleware(IUserRepository userRepository) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var isConversionSuccessful = int.TryParse(context.User.FindFirstValue(TokenClaimNames.Id), out var userId);
        var isBlocked = await userRepository.IsBanned(userId, context.RequestAborted);

        if (isBlocked && isConversionSuccessful)
        {
            throw new UserBannedException();
        }
        await next(context);
    }
}
