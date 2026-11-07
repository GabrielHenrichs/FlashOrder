using FlashOrder.Application.DTOs.Commands;
using FlashOrder.Application.DTOs.Responses;

namespace FlashOrder.Application.UseCases.Trades;

public interface ISellStockUseCase
{
    Task<TransactionResponse> ExecuteAsync(SellStockCommand command, CancellationToken ct);
}
