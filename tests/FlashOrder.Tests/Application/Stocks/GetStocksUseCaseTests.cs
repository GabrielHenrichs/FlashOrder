using FlashOrder.Application.UseCases.Stocks;
using FlashOrder.Domain.Entities;
using FlashOrder.Tests.Fakes;

namespace FlashOrder.Tests.Application.Stocks;

public class GetStocksUseCaseTests
{
    [Fact]
    public async Task Execute_ShouldReturnMappedStocks()
    {
        var repository = new FakeStockRepository();
        await repository.AddAsync(Stock.Create("VALE3", "Vale", 60m), CancellationToken.None);
        await repository.AddAsync(Stock.Create("PETR4", "Petrobras", 30m), CancellationToken.None);
        var useCase = new GetStocksUseCase(repository);

        var result = await useCase.ExecuteAsync(CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.Contains(result, x => x.Code == "PETR4" && x.CompanyName == "Petrobras" && x.CurrentPrice == 30m);
        Assert.Contains(result, x => x.Code == "VALE3" && x.CompanyName == "Vale" && x.CurrentPrice == 60m);
    }

}
