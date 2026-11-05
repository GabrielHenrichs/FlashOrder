using FlashOrder.Domain.Exceptions;

namespace FlashOrder.Domain.Entities;

public sealed class Position
{
    public string StockCode { get; }
    public int Quantity { get; private set; }

    public Position(string stockCode, int quantity)
    {
        if (string.IsNullOrWhiteSpace(stockCode))
            throw new DomainException("Código da ação é obrigatório.");

        if (quantity < 0)
            throw new DomainException("Quantidade não pode ser negativa.");

        StockCode = stockCode.Trim().ToUpperInvariant();
        Quantity = quantity;
    }

    public void Increase(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantidade deve ser maior que zero.");

        Quantity += quantity;
    }

    public void Decrease(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantidade deve ser maior que zero.");

        if (quantity > Quantity)
            throw new DomainException("Quantidade insuficiente para venda.");

        Quantity -= quantity;
    }
}
