using FlashOrder.Application.UseCases.Portfolio;
using FlashOrder.Domain.Entities;
using FlashOrder.UnitTests.Fakes;

namespace FlashOrder.UnitTests.Application.Portfolio;

public class GetPositionByStockCodeUseCaseTests
{
    [Fact]
    public async Task Execute_ShouldReturnPositionByStockCode()
    {
        var position = new Position("AAPL", 150);

        var repository = new FakePositionRepository();
        await repository.UpsertAsync(position, CancellationToken.None);

        var useCase = new GetPositionByStockCodeUseCase(repository);

        var result = await useCase.ExecuteAsync(position.StockCode, CancellationToken.None);

        Assert.Equal(result.StockCode, position.StockCode);
        Assert.Equal(result.Quantity, position.Quantity);
    }

    [Fact]
    public async Task Execute_WhenPositionDoesNotExist_ShouldThrowApplicationException()
    {
        var repository = new FakePositionRepository();
        var useCase = new GetPositionByStockCodeUseCase(repository);

        var ex = await Assert.ThrowsAsync<FlashOrder.Application.Exceptions.ApplicationException>(() =>
            useCase.ExecuteAsync("NONEXISTENT", CancellationToken.None));

        Assert.Equal("Posição não encontrada.", ex.Message);
    }
}
