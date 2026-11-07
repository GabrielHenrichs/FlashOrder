using FlashOrder.Application.UseCases.Stocks;
using FlashOrder.Domain.Entities;
using FlashOrder.Tests.Fakes;

namespace FlashOrder.Tests.Application.Stocks;

public class GetStockByCodeUseCaseTests
{
    [Fact]
    public async Task Execute_ShouldReturnStockByCode()
    {
        var stock = Stock.Create("ITUB4", "Itaú Unibanco", 40m);

        var repository = new FakeStockRepository();
        await repository.AddAsync(stock, CancellationToken.None);

        var useCase = new GetStockByCodeUseCase(repository);

        var result = await useCase.ExecuteAsync(stock.Code, CancellationToken.None);

        Assert.Equal(result.Code, stock.Code);
        Assert.Equal(result.CompanyName, stock.CompanyName);
        Assert.Equal(result.CurrentPrice, stock.CurrentPrice);
    }

    [Fact]
    public async Task Execute_WhenStockDoesNotExist_ShouldThrowApplicationException()
    {
        var repository = new FakeStockRepository();
        var useCase = new GetStockByCodeUseCase(repository);

        var ex = await Assert.ThrowsAsync<FlashOrder.Application.Exceptions.ApplicationException>(() =>
            useCase.ExecuteAsync("NONEXISTENT", CancellationToken.None));

        Assert.Equal("Ação não encontrada.", ex.Message);
    }
}
