using System.Collections.Concurrent;
using FlashOrder.Application.Abstractions;
using FlashOrder.Domain.Entities;

namespace FlashOrder.Infra.Data.Repositories;

public sealed class TransactionRepository : ITransactionRepository
{
    private readonly ConcurrentBag<Transaction> _transactions = [];

    public Task AddAsync(Transaction transaction, CancellationToken ct)
    {
        _transactions.Add(transaction);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyCollection<Transaction>> ListAsync(CancellationToken ct)
        => Task.FromResult((IReadOnlyCollection<Transaction>)_transactions.ToArray());

    public Task<IReadOnlyCollection<Transaction>> ListAsync(string stockCode, CancellationToken ct)
    {
        var transactions = _transactions
            .Where(x => x.StockCode.Equals(stockCode, StringComparison.OrdinalIgnoreCase))
            .ToArray();

        return Task.FromResult((IReadOnlyCollection<Transaction>)transactions);
    }
}
