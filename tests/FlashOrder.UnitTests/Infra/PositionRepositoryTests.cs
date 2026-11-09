using FlashOrder.Domain.Entities;
using FlashOrder.Infra.Data.Repositories;

namespace FlashOrder.UnitTests.Infra;

public class PositionRepositoryTests
{
    [Fact]
    public async Task UpsertAsync_ShouldReplaceExistingPosition()
    {
        var repository = new PositionRepository();
        await repository.UpsertAsync(new Position("PETR4", 10), CancellationToken.None);
        await repository.UpsertAsync(new Position("PETR4", 5), CancellationToken.None);

        var result = await repository.GetByStockCodeAsync("PETR4", CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(5, result.Quantity);
    }

    [Fact]
    public async Task ListAsync_ShouldReturnAllPositions()
    {
        var repository = new PositionRepository();
        await repository.UpsertAsync(new Position("PETR4", 10), CancellationToken.None);
        await repository.UpsertAsync(new Position("VALE3", 20), CancellationToken.None);

        var result = await repository.ListAsync(CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.Contains(result, p => p.StockCode == "PETR4" && p.Quantity == 10);
        Assert.Contains(result, p => p.StockCode == "VALE3" && p.Quantity == 20);
    }
}
