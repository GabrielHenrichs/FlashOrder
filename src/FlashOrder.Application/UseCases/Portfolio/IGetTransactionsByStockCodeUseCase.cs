using FlashOrder.Application.DTOs.Responses;

namespace FlashOrder.Application.UseCases.Portfolio;

public interface IGetTransactionsByStockCodeUseCase
{
    Task<IReadOnlyCollection<TransactionResponse>> ExecuteAsync(string stockCode, CancellationToken ct);
}
