﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API.Extensions.Auth
{
    public static class JwtBearerConfiguration
    {
        public static void LoadConfigurationForJwtBearer(this AuthenticationBuilder authenticationBuilder,
            IConfiguration configuration)
        {
            authenticationBuilder.AddJwtBearer(
                options =>
                {
                    var securityKeyBytes = Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? throw new ArgumentNullException());
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(securityKeyBytes)
                    };
                });
        }
    }
}