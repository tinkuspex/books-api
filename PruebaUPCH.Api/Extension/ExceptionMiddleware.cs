using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace PruebaUPCH.Api.Extension
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var traceId = Activity.Current?.Id ?? context.TraceIdentifier;
            _logger.LogError(exception, "Unhandled exception occurred. TraceId: {TraceId}", traceId);

            var (status, problemDetails) = CreateProblemDetails(exception, context, traceId);

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = status;

            var json = JsonSerializer.Serialize(
                problemDetails,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
            );

            await context.Response.WriteAsync(json);
        }

        private (int statusCode, ProblemDetails details) CreateProblemDetails(Exception ex, HttpContext context, string traceId)
        {
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var title = "Error interno del servidor";

            if (ex is KeyNotFoundException)
            {
                statusCode = (int)HttpStatusCode.NotFound;
                title = "Recurso no encontrado";
            }
            else if (ex is UnauthorizedAccessException)
            {
                statusCode = (int)HttpStatusCode.Unauthorized;
                title = "No autorizado";
            }
            else if (ex.GetType().Name == "ValidationException") // FluentValidation
            {
                statusCode = (int)HttpStatusCode.BadRequest;

                var validationErrors = ex
                    .GetType()
                    .GetProperty("Errors")
                    ?.GetValue(ex) as IEnumerable<object>;

                var errorDict = new Dictionary<string, string[]>();

                if (validationErrors != null)
                {
                    foreach (var error in validationErrors)
                    {
                        var property = error.GetType().GetProperty("PropertyName")?.GetValue(error)?.ToString() ?? "";
                        var message = error.GetType().GetProperty("ErrorMessage")?.GetValue(error)?.ToString() ?? "";

                        if (!errorDict.ContainsKey(property))
                            errorDict[property] = new[] { message };
                    }
                }

                return
                (
                    statusCode,
                    new ValidationProblemDetails(errorDict)
                    {
                        Title = "Error de validación",
                        Detail = ex.Message,
                        Status = statusCode,
                        Instance = context.Request.Path,
                        Extensions = { ["traceId"] = traceId }
                    }
                );
            }

            var problem = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = ex.Message,
                Instance = context.Request.Path
            };

            problem.Extensions["traceId"] = traceId;

            return (statusCode, problem);
        }
    }
}
