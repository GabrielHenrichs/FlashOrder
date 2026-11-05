using FlashOrder.Domain.Entities;

namespace FlashOrder.Application.Abstractions;

public interface IPositionRepository
{
    Task<Position?> GetByStockCodeAsync(string stockCode, CancellationToken ct);

    Task UpsertAsync(Position position, CancellationToken ct);

    Task<IReadOnlyCollection<Position>> ListAsync(CancellationToken ct);
}
