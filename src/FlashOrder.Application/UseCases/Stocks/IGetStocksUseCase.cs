using FlashOrder.Application.DTOs.Responses;

namespace FlashOrder.Application.UseCases.Stocks;

public interface IGetStocksUseCase
{
    Task<IReadOnlyCollection<StockResponse>> ExecuteAsync(CancellationToken ct);
}
