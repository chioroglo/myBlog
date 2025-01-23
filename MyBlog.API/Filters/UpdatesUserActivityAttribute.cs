using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;
using MyBlog.Common;
using MyBlog.Service.Abstract;

namespace MyBlog.API.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class UpdatesUserActivityAttribute : ActionFilterAttribute
{
    public override void OnResultExecuted(ResultExecutedContext context)
    {
        var cancellationToken = context.HttpContext.RequestAborted;
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        var userService = context.HttpContext.RequestServices.GetService<IUserService>();
        if (int.TryParse(context.HttpContext.User.FindFirstValue(TokenClaimNames.Id), out var userId))
        {
            userService?.UpdateLastActivity(userId, cancellationToken);
        }

    }
}