using FlashOrder.Domain.Entities;

namespace FlashOrder.Tests.Fakes;

public class FakeStockRepositoryTests
{
    [Fact]
    public async Task AddAsync_ShouldStoreStockAndAllowExistsLookup()
    {
        var repository = new FakeStockRepository();

        await repository.AddAsync(Stock.Create("PETR4", "Petrobras", 30m), CancellationToken.None);

        Assert.True(await repository.ExistsAsync("PETR4", CancellationToken.None));
    }
}
