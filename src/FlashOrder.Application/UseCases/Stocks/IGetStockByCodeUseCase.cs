using FlashOrder.Application.DTOs.Responses;

namespace FlashOrder.Application.UseCases.Stocks;

public interface IGetStockByCodeUseCase
{
    Task<StockResponse> ExecuteAsync(string code, CancellationToken ct);
}
