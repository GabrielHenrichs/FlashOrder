namespace FlashOrder.Application.DTOs.Responses;

public sealed record PositionResponse(
    string StockCode,
    int Quantity);
