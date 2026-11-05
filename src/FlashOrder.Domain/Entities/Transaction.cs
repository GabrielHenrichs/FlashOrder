using FlashOrder.Domain.Enums;
using FlashOrder.Domain.Exceptions;

namespace FlashOrder.Domain.Entities;

public sealed class Transaction
{
    public string StockCode { get; }
    public TransactionType Type { get; }
    public int Quantity { get; }
    public decimal UnitPrice { get; }
    public decimal TotalValue { get; }
    public DateTime OccurredAt { get; }

    private Transaction(string stockCode, TransactionType type, int quantity, decimal unitPrice, DateTime occurredAt)
    {
        StockCode = stockCode;
        Type = type;
        Quantity = quantity;
        UnitPrice = unitPrice;
        TotalValue = unitPrice * quantity;
        OccurredAt = occurredAt;
    }

    public static Transaction Create(string stockCode, TransactionType type, int quantity, decimal unitPrice, DateTime occurredAtUtc)
    {
        if (string.IsNullOrWhiteSpace(stockCode))
            throw new DomainException("Código da ação é obrigatório.");

        if (quantity <= 0)
            throw new DomainException("Quantidade deve ser maior que zero.");

        if (unitPrice <= 0)
            throw new DomainException("Preço unitário deve ser maior que zero.");

        if (occurredAtUtc.Kind != DateTimeKind.Utc)
            throw new DomainException("Data da transação deve estar em UTC.");

        return new Transaction(stockCode.Trim().ToUpperInvariant(), type, quantity, unitPrice, occurredAtUtc);
    }
}
