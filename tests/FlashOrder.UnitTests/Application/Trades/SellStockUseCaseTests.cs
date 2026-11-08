using FlashOrder.Application.DTOs.Commands;
using FlashOrder.Application.UseCases.Trades;
using FlashOrder.Domain.Entities;
using FlashOrder.UnitTests.Fakes;

namespace FlashOrder.UnitTests.Application.Trades;

public class SellStockUseCaseTests
{
    [Fact]
    public async Task Execute_WithValidInput_ShouldDecreasePositionAndCreateTransaction()
    {
        var stocks = new FakeStockRepository();
        var positions = new FakePositionRepository();
        var transactions = new FakeTransactionRepository();
        await stocks.AddAsync(Stock.Create("PETR4", "Petrobras", 30m), CancellationToken.None);
        await positions.UpsertAsync(new Position("PETR4", 10), CancellationToken.None);
        var sut = new SellStockUseCase(stocks, positions, transactions);

        var tx = await sut.ExecuteAsync(new SellStockCommand("PETR4", 4), CancellationToken.None);

        Assert.Equal("Sell", tx.Type);
        Assert.Equal(120m, tx.TotalValue);
        Assert.Equal(6, (await positions.GetByStockCodeAsync("PETR4", CancellationToken.None))!.Quantity);
        Assert.Single(await transactions.ListAsync(stockCode: null, CancellationToken.None));
    }

    [Fact]
    public async Task Execute_WithInsufficientPosition_ShouldThrowApplicationExceptionAndNotPersistTransaction()
    {
        var stocks = new FakeStockRepository();
        var positions = new FakePositionRepository();
        var transactions = new FakeTransactionRepository();
        await stocks.AddAsync(Stock.Create("PETR4", "Petrobras", 30m), CancellationToken.None);
        await positions.UpsertAsync(new Position("PETR4", 5), CancellationToken.None);
        var sut = new SellStockUseCase(stocks, positions, transactions);

        await Assert.ThrowsAsync<FlashOrder.Application.Exceptions.ApplicationException>(() =>
            sut.ExecuteAsync(new SellStockCommand("PETR4", 6), CancellationToken.None));

        Assert.Empty(await transactions.ListAsync(stockCode: null, CancellationToken.None));
    }

    [Fact]
    public async Task Execute_WithUnknownStock_ShouldThrowApplicationExceptionAndNotPersistTransaction()
    {
        var stocks = new FakeStockRepository();
        var positions = new FakePositionRepository();
        var transactions = new FakeTransactionRepository();
        var sut = new SellStockUseCase(stocks, positions, transactions);

        var ex = await Assert.ThrowsAsync<FlashOrder.Application.Exceptions.ApplicationException>(() =>
            sut.ExecuteAsync(new SellStockCommand("PETR4", 1), CancellationToken.None));

        Assert.Equal("Ação não encontrada.", ex.Message);

        Assert.Empty(await transactions.ListAsync(stockCode: null, CancellationToken.None));
    }

    [Fact]
    public async Task Execute_WithNonPositiveQuantity_ShouldThrowApplicationExceptionAndNotPersistTransaction()
    {
        var stocks = new FakeStockRepository();
        var positions = new FakePositionRepository();
        var transactions = new FakeTransactionRepository();
        await stocks.AddAsync(Stock.Create("PETR4", "Petrobras", 30m), CancellationToken.None);
        await positions.UpsertAsync(new Position("PETR4", 10), CancellationToken.None);
        var sut = new SellStockUseCase(stocks, positions, transactions);

        await Assert.ThrowsAsync<FlashOrder.Application.Exceptions.ApplicationException>(() =>
            sut.ExecuteAsync(new SellStockCommand("PETR4", 0), CancellationToken.None));

        Assert.Empty(await transactions.ListAsync(stockCode: null, CancellationToken.None));
    }
}
