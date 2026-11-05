using FlashOrder.Domain.Entities;
using FlashOrder.Domain.Enums;

namespace FlashOrder.Tests.Domain;

public class TransactionTests
{
    [Fact]
    public void Create_ShouldCalculateTotalValue()
    {
        var tx = Transaction.Create("PETR4", TransactionType.Buy, 10, 30m, DateTime.UtcNow);

        Assert.Equal(300m, tx.TotalValue);
    }
}
