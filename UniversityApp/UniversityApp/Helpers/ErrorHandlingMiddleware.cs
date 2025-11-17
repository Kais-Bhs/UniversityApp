// ---------------------------------------------------------------
// Copyright (c ) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using System.Net;
using System.Text.Json;
using DTOs.Common;
using NLog;

namespace UniversityApp.Helpers
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly NLog.ILogger _logger = LogManager.GetCurrentClassLogger();

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            var message = "An unexpected error occurred.";
            var errors = new List<string>();

            switch (exception)
            {
                case KeyNotFoundException keyNotFoundEx:
                    statusCode = HttpStatusCode.NotFound;
                    message = keyNotFoundEx.Message;
                    _logger.Warn(keyNotFoundEx, "Resource not found: {Message}", keyNotFoundEx.Message);
                    break;

                case UnauthorizedAccessException unauthorizedEx:
                    statusCode = HttpStatusCode.Unauthorized;
                    message = unauthorizedEx.Message;
                    _logger.Warn(unauthorizedEx, "Unauthorized access attempt: {Message}", unauthorizedEx.Message);
                    break;

                case InvalidOperationException invalidOpEx:
                    statusCode = HttpStatusCode.BadRequest;
                    message = invalidOpEx.Message;
                    _logger.Warn(invalidOpEx, "Invalid operation: {Message}", invalidOpEx.Message);
                    break;

                case ArgumentException argEx:
                    statusCode = HttpStatusCode.BadRequest;
                    message = argEx.Message;
                    _logger.Warn(argEx, "Invalid argument: {Message}", argEx.Message);
                    break;

                default:
                    _logger.Error(exception, "Unhandled exception occurred: {Message}", exception.Message);
                    errors.Add(exception.Message);
                    if (exception.InnerException != null)
                    {
                        errors.Add($"Inner: {exception.InnerException.Message}");
                    }
                    break;
            }

            _logger.Error("Request Path: {Path}, Method: {Method}, User: {User}",
                context.Request.Path,
                context.Request.Method,
                context.User?.Identity?.Name ?? "Anonymous");

            var response = ApiResponse<object>.ErrorResponse(message, errors);
            var payload = JsonSerializer.Serialize(response);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(payload);
        }
    }

    public static class ErrorHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}