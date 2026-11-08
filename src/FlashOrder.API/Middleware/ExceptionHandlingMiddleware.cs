using FlashOrder.API.Contracts.Responses;
using FlashOrder.Application.Exceptions;

namespace FlashOrder.API.Middleware;

public sealed class ExceptionHandlingMiddleware
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
        catch (Application.Exceptions.ApplicationException ex)
        {
            _logger.LogWarning(
                ex,
                "Application exception {ErrorCode} on {Method} {Path} traceId={TraceId}",
                ex.ErrorCode,
                context.Request.Method,
                context.Request.Path.Value,
                context.TraceIdentifier);

            var statusCode = ex.ErrorCode switch
            {
                ApplicationErrorCode.Conflict => StatusCodes.Status409Conflict,
                ApplicationErrorCode.NotFound => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status422UnprocessableEntity
            };

            await WriteErrorAsync(context, statusCode, "application_error", ex.Message, null);
        }
        catch (Domain.Exceptions.DomainException ex)
        {
            _logger.LogWarning(
                ex,
                "Domain exception on {Method} {Path} traceId={TraceId}",
                context.Request.Method,
                context.Request.Path.Value,
                context.TraceIdentifier);

            await WriteErrorAsync(context, StatusCodes.Status422UnprocessableEntity, "domain_error", ex.Message, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unhandled exception on {Method} {Path} traceId={TraceId}",
                context.Request.Method,
                context.Request.Path.Value,
                context.TraceIdentifier);

            await WriteErrorAsync(context, StatusCodes.Status500InternalServerError, "internal_error", "Unexpected error.", ex.Message);
        }
    }

    private static async Task WriteErrorAsync(HttpContext context, int statusCode, string code, string message, string? details)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var payload = new ErrorResponse(code, message, details, context.TraceIdentifier);
        await context.Response.WriteAsJsonAsync(payload);
    }
}
