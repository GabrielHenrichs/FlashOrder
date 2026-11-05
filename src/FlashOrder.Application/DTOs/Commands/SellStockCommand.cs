namespace FlashOrder.Application.DTOs.Commands;

public sealed record SellStockCommand(
    string StockCode,
    int Quantity);
