using FlashOrder.Domain.Entities;
using FlashOrder.Infra.Data.Repositories;

namespace FlashOrder.UnitTests.Infra;

public class StockRepositoryTests
{
    [Fact]
    public async Task AddAsync_WhenCodeAlreadyExists_ShouldThrowInvalidOperationException()
    {
        var repository = new StockRepository();
        await repository.AddAsync(Stock.Create("PETR4", "Petrobras", 30m), CancellationToken.None);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            repository.AddAsync(Stock.Create("PETR4", "Outra", 40m), CancellationToken.None));
    }

    [Fact]
    public async Task ExistsAsync_ShouldBeCaseInsensitive()
    {
        var repository = new StockRepository();
        await repository.AddAsync(Stock.Create("PETR4", "Petrobras", 30m), CancellationToken.None);

        var exists = await repository.ExistsAsync("petr4", CancellationToken.None);

        Assert.True(exists);
    }

    [Fact]
    public async Task GetByCodeAsync_ShouldReturnStockIfExists()
    {
        var repository = new StockRepository();

        var stock = Stock.Create("ITUB4", "Itaú Unibanco", 30m);
        await repository.AddAsync(stock, CancellationToken.None);

        var result = await repository.GetByCodeAsync("ITUB4", CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(stock.Code, result.Code);
        Assert.Equal(stock.CompanyName, result.CompanyName);
        Assert.Equal(stock.CurrentPrice, result.CurrentPrice);
    }

    [Fact]
    public async Task GetByCodeAsync_ShouldReturnNullIfNotFound()
    {
        var repository = new StockRepository();

        var result = await repository.GetByCodeAsync("NONEXISTENT", CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task ListAsync_ShouldReturnAllStocksInOrder()
    {
        var repository = new StockRepository();

        await repository.AddAsync(Stock.Create("PETR4", "Petrobras", 30m), CancellationToken.None);
        await repository.AddAsync(Stock.Create("VALE3", "Vale", 40m), CancellationToken.None);
        await repository.AddAsync(Stock.Create("ITUB4", "Itaú Unibanco", 50m), CancellationToken.None);

        var result = await repository.ListAsync(CancellationToken.None);

        Assert.Equal(3, result.Count);
        Assert.Equal("ITUB4", result.ElementAt(0).Code);
        Assert.Equal("PETR4", result.ElementAt(1).Code);
        Assert.Equal("VALE3", result.ElementAt(2).Code);
    }
}
