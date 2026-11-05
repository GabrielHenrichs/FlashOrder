using FlashOrder.Domain.Exceptions;

namespace FlashOrder.Domain.Entities;

public sealed class Stock
{
    public string Code { get; }
    public string CompanyName { get; }
    public decimal CurrentPrice { get; }

    private Stock(string code, string companyName, decimal currentPrice)
    {
        Code = code;
        CompanyName = companyName;
        CurrentPrice = currentPrice;
    }

    public static Stock Create(string code, string companyName, decimal currentPrice)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new DomainException("Código da ação é obrigatório.");

        if (string.IsNullOrWhiteSpace(companyName))
            throw new DomainException("Nome da empresa é obrigatório.");

        if (currentPrice <= 0)
            throw new DomainException("Preço atual deve ser maior que zero.");

        return new Stock(
            code.Trim().ToUpperInvariant(), companyName.Trim(), currentPrice);
    }
}
