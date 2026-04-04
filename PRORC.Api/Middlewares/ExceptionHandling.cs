using System.Net;
using System.Text.Json;

namespace PRORC.Api.Middlewares
{
    public class ExceptionHandling(RequestDelegate next, ILogger<ExceptionHandling> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionHandling> _logger = logger;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred in the API");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = exception switch
            {
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                ArgumentException => (int)HttpStatusCode.BadRequest,
                InvalidOperationException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var correlationId = context.Items["X-Correlation-Id"]?.ToString();

            var response = new
            {
                success = false,
                message = exception.Message,
                statusCode,
                correlationId
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}
