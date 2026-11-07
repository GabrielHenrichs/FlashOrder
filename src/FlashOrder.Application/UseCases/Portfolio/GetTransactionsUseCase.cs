using FlashOrder.Application.Abstractions;
using FlashOrder.Application.DTOs.Responses;

namespace FlashOrder.Application.UseCases.Portfolio;

public sealed class GetTransactionsUseCase(ITransactionRepository transactions) : IGetTransactionsUseCase
{
    public async Task<IReadOnlyCollection<TransactionResponse>> ExecuteAsync(CancellationToken ct)
    {
        var all = await transactions.ListAsync(ct);

        return all
            .OrderByDescending(x => x.OccurredAt)
            .Select(x => new TransactionResponse(
                x.StockCode,
                x.Type.ToString(),
                x.Quantity,
                x.UnitPrice,
                x.TotalValue,
                x.OccurredAt))
            .ToArray();
    }
}
