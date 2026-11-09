using FlashOrder.Domain.Entities;
using FlashOrder.Domain.Enums;
using FlashOrder.Infra.Data.Repositories;

namespace FlashOrder.UnitTests.Infra;

public class TransactionRepositoryTests
{
    [Fact]
    public async Task AddAsync_ShouldPersistTransactions()
    {
        var repository = new TransactionRepository();
        await repository.AddAsync(Transaction.Create("PETR4", TransactionType.Buy, 2, 30m, DateTime.UtcNow), CancellationToken.None);

        var result = await repository.ListAsync(stockCode: null, CancellationToken.None);

        Assert.Single(result);
    }

    [Fact]
    public async Task ListAsync_ShouldFilterByStockCode()
    {
        var repository = new TransactionRepository();
        await repository.AddAsync(Transaction.Create("ITUB4", TransactionType.Buy, 2, 30m, DateTime.UtcNow), CancellationToken.None);
        await repository.AddAsync(Transaction.Create("VALE3", TransactionType.Sell, 1, 100m, DateTime.UtcNow), CancellationToken.None);
        
        var result = await repository.ListAsync(stockCode: "ITUB4", CancellationToken.None);
        
        Assert.Single(result);
        Assert.Equal("ITUB4", result.First().StockCode);
    }
}
