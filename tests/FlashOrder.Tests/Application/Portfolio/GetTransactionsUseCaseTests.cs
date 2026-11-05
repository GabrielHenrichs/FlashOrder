using FlashOrder.Application.UseCases.Portfolio;
using FlashOrder.Domain.Entities;
using FlashOrder.Domain.Enums;
using FlashOrder.Tests.Fakes;

namespace FlashOrder.Tests.Application.Portfolio;

public class GetTransactionsUseCaseTests
{
    [Fact]
    public async Task Execute_ShouldReturnTransactionsOrderedByOccurredAtDescending()
    {
        var transactions = new FakeTransactionRepository();
        await transactions.AddAsync(Transaction.Create("PETR4", TransactionType.Buy, 1, 10m, DateTime.UtcNow.AddMinutes(-2)), CancellationToken.None);
        await transactions.AddAsync(Transaction.Create("PETR4", TransactionType.Sell, 1, 12m, DateTime.UtcNow), CancellationToken.None);

        var sut = new GetTransactionsUseCase(transactions);

        var result = await sut.ExecuteAsync(CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.Equal("Sell", result.First().Type);
        Assert.Equal("Buy", result.Last().Type);
    }
}
