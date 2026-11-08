using FlashOrder.Application.Abstractions;
using FlashOrder.Domain.Entities;

namespace FlashOrder.UnitTests.Fakes;

public sealed class FakePositionRepository : IPositionRepository
{
    private readonly Dictionary<string, Position> _positions = new(StringComparer.OrdinalIgnoreCase);

    public Task<Position?> GetByStockCodeAsync(string stockCode, CancellationToken ct)
    {
        _positions.TryGetValue(stockCode, out var position);
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
