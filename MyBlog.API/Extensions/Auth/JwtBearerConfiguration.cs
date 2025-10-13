using System.Net;
using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MyBlog.API.Middlewares.Models;
using MyBlog.Common.Options;

namespace MyBlog.API.Extensions.Auth
{
    public static class JwtBearerConfiguration
    {
        public static void LoadConfigurationForJwtBearer(this AuthenticationBuilder authenticationBuilder, IConfiguration configuration)
        {

            authenticationBuilder.AddJwtBearer(
                options =>
                {
                    var jwtOptions = configuration.GetSection("Jwt").Get<JsonWebTokenOptions>();
                    var rsa = RSA.Create();
                    rsa.ImportFromPem(jwtOptions!.PublicKey);
                    
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience,
                        IssuerSigningKey = new RsaSecurityKey(rsa)
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = async context =>
                        {
                            context.HandleResponse();
                            const int statusCode = (int)HttpStatusCode.Unauthorized;

                            context.Response.StatusCode = statusCode;

                            var error = new ErrorDetails(statusCode, "Authentication failed!");

                            var json = JsonSerializer.SerializeToUtf8Bytes(error);
                            await context.Response.BodyWriter.WriteAsync(json);
                        }
                    };
                });
        }
    }
}