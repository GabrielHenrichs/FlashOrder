using System.Collections.Concurrent;
using FlashOrder.Application.Abstractions;
using FlashOrder.Domain.Entities;

namespace FlashOrder.Infra.Data.Repositories;

public sealed class PositionRepository : IPositionRepository
{
    private readonly ConcurrentDictionary<string, Position> _positions = new(StringComparer.OrdinalIgnoreCase);

    public Task<Position?> GetByStockCodeAsync(string stockCode, CancellationToken ct)
    {
        _positions.TryGetValue(stockCode.Trim().ToUpperInvariant(), out var position);
        return Task.FromResult(position);
    }

    public Task UpsertAsync(Position position, CancellationToken ct)
    {
        _positions[position.StockCode] = position;
        return Task.CompletedTask;
    }

    public Task<IReadOnlyCollection<Position>> ListAsync(CancellationToken ct)
        => Task.FromResult((IReadOnlyCollection<Position>)_positions.Values.ToArray());
}
