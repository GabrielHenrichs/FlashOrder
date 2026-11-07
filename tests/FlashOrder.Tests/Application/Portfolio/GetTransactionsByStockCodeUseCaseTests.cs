using FlashOrder.Application.UseCases.Portfolio;
using FlashOrder.Domain.Entities;
using FlashOrder.Domain.Enums;
using FlashOrder.Tests.Fakes;

namespace FlashOrder.Tests.Application.Portfolio;

public class GetTransactionsByStockCodeUseCaseTests
{
    [Fact]
    public async Task Execute_ShouldReturnTransactionsOrderedByOccurredAtDescending()
    {
        var transactions = new FakeTransactionRepository();
        await transactions.AddAsync(Transaction.Create("PETR4", TransactionType.Buy, 1, 10m, DateTime.UtcNow.AddMinutes(-2)), CancellationToken.None);
        await transactions.AddAsync(Transaction.Create("PETR4", TransactionType.Sell, 1, 12m, DateTime.UtcNow), CancellationToken.None);
        await transactions.AddAsync(Transaction.Create("ITUB4", TransactionType.Buy, 10, 40m, DateTime.UtcNow), CancellationToken.None);

        var sut = new GetTransactionsByStockCodeUseCase(transactions);

        var result = await sut.ExecuteAsync("ITUB4", CancellationToken.None);

        Assert.Equal(1, result.Count);
        Assert.Equal("Buy", result.First().Type);
        Assert.Equal("ITUB4", result.First().StockCode);
    }
}
