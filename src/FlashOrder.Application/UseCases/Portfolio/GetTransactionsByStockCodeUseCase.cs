using FlashOrder.Application.Abstractions;
using FlashOrder.Application.DTOs.Responses;

namespace FlashOrder.Application.UseCases.Portfolio;

public sealed class GetTransactionsByStockCodeUseCase(ITransactionRepository transactions) : IGetTransactionsByStockCodeUseCase
{
    public async Task<IReadOnlyCollection<TransactionResponse>> ExecuteAsync(string stockCode, CancellationToken ct)
    {
        var all = await transactions.ListAsync(stockCode, ct);

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
