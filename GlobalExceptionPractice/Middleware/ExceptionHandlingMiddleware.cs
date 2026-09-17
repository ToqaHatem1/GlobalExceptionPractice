using GlobalExceptionPractice.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace GlobalExceptionPractice.Middleware
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
                //continue pipeline
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex, 
                    "An unhandled exception has occurred while executing the request."
                );
                // handle exception
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            string message;
            if (exception is NotFoundException)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                message = exception.Message;
            }
            else if (exception is BadRequestException)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                message = exception.Message;
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                message = "An unexpected error occurred.";
            }
            var problemDetails = new ProblemDetails
            {
                Status = context.Response.StatusCode,
                Title = message,
                Detail = message,
                Instance = context.Request.Path //endpoint(URL) that has the problem
            };

            await context.Response.WriteAsJsonAsync(problemDetails); //object -> json, and write it in HTTP response body
        }
    }
}
