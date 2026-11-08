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
}
