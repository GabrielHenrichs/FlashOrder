using FlashOrder.Application.DTOs.Commands;
using FlashOrder.Application.UseCases.Stocks;
using FlashOrder.Domain.Entities;
using FlashOrder.Tests.Fakes;

namespace FlashOrder.Tests.Application.Stocks;

public class CreateStockUseCaseTests
{
    [Fact]
    public async Task Execute_WhenStockIsValid_ShouldPersistStock()
    {
        var repository = new FakeStockRepository();
        var useCase = new CreateStockUseCase(repository);
        var command = new CreateStockCommand("PETR4", "Petrobras", 30m);

        var result = await useCase.ExecuteAsync(command, CancellationToken.None);

        Assert.Equal("PETR4", result.Code);
        Assert.Equal("Petrobras", result.CompanyName);
        Assert.Equal(30m, result.CurrentPrice);
        Assert.True(await repository.ExistsAsync("PETR4", CancellationToken.None));
    }

    [Fact]
    public async Task Execute_WhenCodeAlreadyExists_ShouldThrowApplicationException()
    {
        var repository = new FakeStockRepository();
        await repository.AddAsync(Stock.Create("PETR4", "Petrobras", 30m), CancellationToken.None);
        var useCase = new CreateStockUseCase(repository);

        var ex = await Assert.ThrowsAsync<FlashOrder.Application.Exceptions.ApplicationException>(() =>
            useCase.ExecuteAsync(new CreateStockCommand("PETR4", "Outra", 40m), CancellationToken.None));

        Assert.Contains("cadastrada", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

}
