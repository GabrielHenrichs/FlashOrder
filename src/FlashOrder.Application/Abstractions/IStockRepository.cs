using FlashOrder.Domain.Entities;

namespace FlashOrder.Application.Abstractions;

public interface IStockRepository
{
    Task<bool> ExistsAsync(string code, CancellationToken ct);

    Task AddAsync(Stock stock, CancellationToken ct);

    Task<Stock?> GetByCodeAsync(string code, CancellationToken ct);

    Task<IReadOnlyCollection<Stock>> ListAsync(CancellationToken ct);
}
