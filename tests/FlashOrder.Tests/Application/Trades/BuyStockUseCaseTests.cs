using FlashOrder.Application.DTOs.Commands;
using FlashOrder.Application.UseCases.Trades;
using FlashOrder.Domain.Entities;
using FlashOrder.Tests.Fakes;

namespace FlashOrder.Tests.Application.Trades;

public class BuyStockUseCaseTests
{
    [Fact]
    public async Task Execute_WithValidInput_ShouldIncreasePositionAndCreateTransaction()
    {
        var stocks = new FakeStockRepository();
        var positions = new FakePositionRepository();
        var transactions = new FakeTransactionRepository();
        await stocks.AddAsync(Stock.Create("PETR4", "Petrobras", 30m), CancellationToken.None);

        var sut = new BuyStockUseCase(stocks, positions, transactions);

        var tx = await sut.ExecuteAsync(new BuyStockCommand("PETR4", 10), CancellationToken.None);

        Assert.Equal("Buy", tx.Type);
        Assert.Equal(300m, tx.TotalValue);
        Assert.Equal(10, (await positions.GetByStockCodeAsync("PETR4", CancellationToken.None))!.Quantity);
        Assert.Single(await transactions.ListAsync(CancellationToken.None));
    }

    [Fact]
    public async Task Execute_WithUnknownStock_ShouldThrowApplicationException()
    {
        var stocks = new FakeStockRepository();
        var positions = new FakePositionRepository();
        var transactions = new FakeTransactionRepository();
        var sut = new BuyStockUseCase(stocks, positions, transactions);

        var ex = await Assert.ThrowsAsync<FlashOrder.Application.Exceptions.ApplicationException>(() =>
            sut.ExecuteAsync(new BuyStockCommand("PETR4", 1), CancellationToken.None));

        Assert.Equal("Ação não encontrada.", ex.Message);

        Assert.Empty(await transactions.ListAsync(CancellationToken.None));
    }

    [Fact]
    public async Task Execute_WithNonPositiveQuantity_ShouldThrowApplicationExceptionAndNotPersistTransaction()
    {
        var stocks = new FakeStockRepository();
        var positions = new FakePositionRepository();
        var transactions = new FakeTransactionRepository();
        await stocks.AddAsync(Stock.Create("PETR4", "Petrobras", 30m), CancellationToken.None);
        var sut = new BuyStockUseCase(stocks, positions, transactions);

        await Assert.ThrowsAsync<FlashOrder.Application.Exceptions.ApplicationException>(() =>
            sut.ExecuteAsync(new BuyStockCommand("PETR4", 0), CancellationToken.None));

        Assert.Empty(await transactions.ListAsync(CancellationToken.None));
    }
}
