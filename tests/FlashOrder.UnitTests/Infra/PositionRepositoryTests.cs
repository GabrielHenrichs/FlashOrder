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
}
