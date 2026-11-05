namespace FlashOrder.Application.DTOs.Responses;

public sealed record TransactionResponse(
    string StockCode,
    string Type,
    int Quantity,
    decimal UnitPrice,
    decimal TotalValue,
    DateTime OccurredAt);
