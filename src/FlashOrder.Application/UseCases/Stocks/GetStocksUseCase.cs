using FlashOrder.Application.Abstractions;
using FlashOrder.Application.DTOs.Responses;

namespace FlashOrder.Application.UseCases.Stocks;

public sealed class GetStocksUseCase(IStockRepository stockRepository) : IGetStocksUseCase
{
    public async Task<IReadOnlyCollection<StockResponse>> ExecuteAsync(CancellationToken ct)
    {
        var stocks = await stockRepository.ListAsync(ct);

        return stocks
            .Select(s => new StockResponse(s.Code, s.CompanyName, s.CurrentPrice))
            .ToArray();
    }
}
