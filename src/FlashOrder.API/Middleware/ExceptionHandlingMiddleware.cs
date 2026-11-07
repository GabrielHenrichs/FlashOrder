using FlashOrder.API.Contracts.Responses;
using FlashOrder.Application.Exceptions;

namespace FlashOrder.API.Middleware;

public sealed class ExceptionHandlingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (FlashOrder.Application.Exceptions.ApplicationException ex)
        {
            var statusCode = ex.ErrorCode switch
            {
                ApplicationErrorCode.Conflict => StatusCodes.Status409Conflict,
                ApplicationErrorCode.NotFound => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status422UnprocessableEntity
            };

            await WriteErrorAsync(context, statusCode, "application_error", ex.Message, null);
        }
        catch (FlashOrder.Domain.Exceptions.DomainException ex)
        {
            await WriteErrorAsync(context, StatusCodes.Status422UnprocessableEntity, "domain_error", ex.Message, null);
        }
        catch (Exception ex)
        {
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
