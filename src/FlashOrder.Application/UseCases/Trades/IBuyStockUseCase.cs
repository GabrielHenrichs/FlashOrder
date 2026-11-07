using FlashOrder.Application.DTOs.Commands;
using FlashOrder.Application.DTOs.Responses;

namespace FlashOrder.Application.UseCases.Trades;

public interface IBuyStockUseCase
{
    Task<TransactionResponse> ExecuteAsync(BuyStockCommand command, CancellationToken ct);
}
