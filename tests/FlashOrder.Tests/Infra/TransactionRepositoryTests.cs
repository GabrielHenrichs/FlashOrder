using FlashOrder.Domain.Entities;
using FlashOrder.Domain.Enums;
using FlashOrder.Infra.Data.Repositories;

namespace FlashOrder.Tests.Infra;

public class TransactionRepositoryTests
{
    [Fact]
    public async Task AddAsync_ShouldPersistTransactions()
    {
        var repository = new TransactionRepository();
        await repository.AddAsync(Transaction.Create("PETR4", TransactionType.Buy, 2, 30m, DateTime.UtcNow), CancellationToken.None);

        var result = await repository.ListAsync(CancellationToken.None);

        Assert.Single(result);
    }
}
