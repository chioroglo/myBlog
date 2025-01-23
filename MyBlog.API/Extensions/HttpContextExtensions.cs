using MyBlog.Common.Utils;

namespace MyBlog.API.Extensions;

public static class HttpContextExtensions
{
    public static void AddRefreshTokenCookie(this HttpContext httpContext, string refreshToken, DateTime expiresAtUtc)
    {
        httpContext.Response.Cookies.Append(
            JwtUtils.CookieRefreshTokenKey,
            refreshToken,
            new CookieOptions
            {
               HttpOnly = true,
               Secure = true,
               Expires = expiresAtUtc,
               SameSite = SameSiteMode.None
            });
    }
}
