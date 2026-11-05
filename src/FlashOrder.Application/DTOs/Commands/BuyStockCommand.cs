namespace FlashOrder.Application.DTOs.Commands;

public sealed record BuyStockCommand(
    string StockCode,
    int Quantity);
