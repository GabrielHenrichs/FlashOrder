using FlashOrder.Application.DTOs.Commands;
using FlashOrder.Application.DTOs.Responses;

namespace FlashOrder.Application.UseCases.Stocks;

public interface ICreateStockUseCase
{
    Task<StockResponse> ExecuteAsync(CreateStockCommand command, CancellationToken ct);
}
