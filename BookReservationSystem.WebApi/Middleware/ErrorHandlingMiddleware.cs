using BookReservationSystem.WebApi.Model;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BookReservationSystem.WebApi.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Log the incoming request
            _logger.LogInformation("Handling request: {Method} {Url}", context.Request.Method, context.Request.Path);

            try
            {
                await _next(context); // Call the next middleware in the pipeline
            }
            catch (Exception ex)
            {
                // Log the exception details
                _logger.LogError(ex, "An unhandled exception occurred while processing the request.");

                await HandleExceptionAsync(context, ex);
            }

            // Log the outgoing response
            _logger.LogInformation("Response sent: {StatusCode}", context.Response.StatusCode);
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            // Log the error message
            _logger.LogError(ex, ex.Message);

            // Default response
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected
            var errorResponse = new ApiErrorResponse((int)code, "An unexpected error occurred.", ex.Message);

            // Handle specific exceptions
            switch (ex)
            {
                case DbUpdateException:
                    code = HttpStatusCode.BadRequest;
                    errorResponse = new ApiErrorResponse((int)code, "A database error occurred. Please check the input data.", ex.Message);
                    break;

                case ArgumentNullException:
                    code = HttpStatusCode.BadRequest;
                    errorResponse = new ApiErrorResponse((int)code, "A required argument was not provided.", ex.Message);
                    break;

                case KeyNotFoundException:
                    code = HttpStatusCode.NotFound;
                    errorResponse = new ApiErrorResponse((int)code, "The requested resource was not found.", ex.Message);
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}
