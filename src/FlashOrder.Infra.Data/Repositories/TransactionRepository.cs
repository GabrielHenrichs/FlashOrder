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
}
