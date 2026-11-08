using FlashOrder.Application.DTOs.Responses;

namespace FlashOrder.Application.UseCases.Portfolio;

public interface IGetTransactionsUseCase
{
    Task<IReadOnlyCollection<TransactionResponse>> ExecuteAsync(string? stockCode, CancellationToken ct);
}
