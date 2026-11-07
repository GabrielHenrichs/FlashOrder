using FlashOrder.Domain.Entities;
using FlashOrder.Domain.Enums;

using FlashOrder.Domain.Exceptions;

namespace FlashOrder.Tests.Domain;

public class TransactionTests
{
    [Fact]
    public void Create_ShouldCalculateTotalValue()
    {
        var tx = Transaction.Create("PETR4", TransactionType.Buy, 10, 30m, DateTime.UtcNow);

        Assert.Equal(300m, tx.TotalValue);
    }

    [Fact]
    public void Create_WithEmptyStockCode_ShouldThrowDomainException()
    {
        var ex = Assert.Throws<DomainException>(() =>
            Transaction.Create("", TransactionType.Buy, 1, 10m, DateTime.UtcNow));

        Assert.Contains("Código", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Create_WithNonPositiveQuantity_ShouldThrowDomainException()
    {
        var ex = Assert.Throws<DomainException>(() =>
            Transaction.Create("PETR4", TransactionType.Buy, 0, 10m, DateTime.UtcNow));

        Assert.Contains("Quantidade", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Create_WithNonPositiveUnitPrice_ShouldThrowDomainException()
    {
        var ex = Assert.Throws<DomainException>(() =>
            Transaction.Create("PETR4", TransactionType.Buy, 1, 0m, DateTime.UtcNow));

        Assert.Contains("Preço", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Create_WithNonUtcDate_ShouldThrowDomainException()
    {
        var localTime = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);

        var ex = Assert.Throws<DomainException>(() =>
            Transaction.Create("PETR4", TransactionType.Buy, 1, 10m, localTime));

        Assert.Contains("UTC", ex.Message, StringComparison.OrdinalIgnoreCase);
    }
}
