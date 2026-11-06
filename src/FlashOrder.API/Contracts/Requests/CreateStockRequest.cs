namespace FlashOrder.API.Contracts.Requests;

public sealed record CreateStockRequest(
    string Code,
    string CompanyName,
    decimal CurrentPrice);
