namespace FlashOrder.Application.DTOs.Responses;

public sealed record StockResponse(
    string Code,
    string CompanyName,
    decimal CurrentPrice);
