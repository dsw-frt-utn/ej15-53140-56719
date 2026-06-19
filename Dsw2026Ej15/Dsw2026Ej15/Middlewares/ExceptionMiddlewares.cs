using Dsw2026Ej15.Domain.Exceptions;
using System.Text.Json;

namespace Dsw2026Ej15.Api.Middlewares;

public class ExceptionMiddlewares
{
        private readonly RequestDelegate _next;

        public ExceptionMiddlewares(RequestDelegate next)
        {
            _next = next;

        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";

                var errorResponse = new
                {
                    errorCode = "VALIDATION_ERROR",
                    message = ex.Message,

                };

                var jsonResponse = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(jsonResponse);
            }
            catch (Exception)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var errorResponse = new
                {
                    errorCode = "INTERNAL_SERVER_ERROR",
                    message = "An error occurred.",

                };

                var jsonResponse = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(jsonResponse);
            }
        }
}

