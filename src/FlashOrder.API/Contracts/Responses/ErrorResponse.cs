namespace FlashOrder.API.Contracts.Responses;

public sealed record ErrorResponse(
    string Code,
    string Message,
    string? Details,
    string TraceId);
