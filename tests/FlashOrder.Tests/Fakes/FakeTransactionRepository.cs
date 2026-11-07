using FlashOrder.Application.Abstractions;
using FlashOrder.Domain.Entities;

namespace FlashOrder.Tests.Fakes;

public sealed class FakeTransactionRepository : ITransactionRepository
{
    private readonly List<Transaction> _transactions = [];

    public Task AddAsync(Transaction transaction, CancellationToken ct)
    {
        _transactions.Add(transaction);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyCollection<Transaction>> ListAsync(string stockCode, CancellationToken ct)
        => Task.FromResult((IReadOnlyCollection<Transaction>)_transactions.Where(x => x.StockCode == stockCode).ToArray());

    public Task<IReadOnlyCollection<Transaction>> ListAsync(CancellationToken ct)
        => Task.FromResult((IReadOnlyCollection<Transaction>)_transactions.ToArray());
}
