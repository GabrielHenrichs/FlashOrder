using FlashOrder.Domain.Entities;

namespace FlashOrder.Application.Abstractions;

public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction, CancellationToken ct);

    Task<IReadOnlyCollection<Transaction>> ListAsync(CancellationToken ct);
}
