namespace FlashOrder.API.Contracts.Requests;

public sealed record TradeRequest(
    string StockCode,
    int Quantity);
