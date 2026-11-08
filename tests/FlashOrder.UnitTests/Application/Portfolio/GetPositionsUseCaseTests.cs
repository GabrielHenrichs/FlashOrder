using FlashOrder.Application.UseCases.Portfolio;
using FlashOrder.Domain.Entities;
using FlashOrder.UnitTests.Fakes;

namespace FlashOrder.UnitTests.Application.Portfolio;

public class GetPositionsUseCaseTests
{
    [Fact]
    public async Task Execute_ShouldReturnOnlyPositivePositionsOrderedByStockCode()
    {
        var positions = new FakePositionRepository();
        await positions.UpsertAsync(new Position("PETR4", 10), CancellationToken.None);
        await positions.UpsertAsync(new Position("VALE3", 0), CancellationToken.None);
        await positions.UpsertAsync(new Position("ABEV3", 2), CancellationToken.None);

        var sut = new GetPositionsUseCase(positions);

        var result = await sut.ExecuteAsync(CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.Equal("ABEV3", result.First().StockCode);
        Assert.Equal("PETR4", result.Last().StockCode);
    }
}
