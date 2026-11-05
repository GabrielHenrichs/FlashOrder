namespace FlashOrder.Application.DTOs.Commands;

public sealed record CreateStockCommand(
    string Code,
    string CompanyName,
    decimal CurrentPrice);
