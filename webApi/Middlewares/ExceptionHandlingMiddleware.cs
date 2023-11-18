﻿using API.Middlewares.Models;
using Common.Exceptions;
using System.Net;
using System.Security.Authentication;

namespace API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                switch (e)
                {
                    case ValidationException:
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    }
                    case AuthenticationException:
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;
                    }
                    case InsufficientPermissionsException:
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        break;
                    }
                    case OperationCanceledException:
                    {
                        return;
                    }
                    default:
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                    }
                }

                await CreateExceptionResponseAsync(context, e);
            }
        }

        private static Task CreateExceptionResponseAsync(HttpContext context, Exception e)
        {
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsync(
                new ErrorDetails(context.Response.StatusCode, e.Message).ToString()
            );
        }
    }
}