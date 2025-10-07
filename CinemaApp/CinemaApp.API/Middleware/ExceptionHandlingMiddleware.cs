using CinemaApp.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace CinemaApp.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

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
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var (statusCode, message) = exception switch
            {
                ValidationException => (HttpStatusCode.UnprocessableEntity, "Ошибка валидации данных:" + exception.Message),
                DbUpdateException => (HttpStatusCode.Conflict, "Ошибка при сохранении данных"),
                _ => (HttpStatusCode.InternalServerError, "Внутренняя ошибка сервера")
            };

            response.StatusCode = (int)statusCode;

            var errorResponse = new
            {
                Error = message,
                Details = statusCode == HttpStatusCode.InternalServerError ? null : exception.Message
            };

            await response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }

}
