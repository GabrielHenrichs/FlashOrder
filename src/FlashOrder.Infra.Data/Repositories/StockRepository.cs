using System.Collections.Concurrent;
using FlashOrder.Application.Abstractions;
using FlashOrder.Domain.Entities;

namespace FlashOrder.Infra.Data.Repositories;

public sealed class StockRepository : IStockRepository
{
    private readonly ConcurrentDictionary<string, Stock> _stocks = new(StringComparer.OrdinalIgnoreCase);

    public Task AddAsync(Stock stock, CancellationToken ct)
    {
        if (!_stocks.TryAdd(stock.Code, stock))
            throw new InvalidOperationException("Stock code already exists.");

        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string code, CancellationToken ct)
        => Task.FromResult(_stocks.ContainsKey(code.Trim().ToUpperInvariant()));

    public Task<Stock?> GetByCodeAsync(string code, CancellationToken ct)
    {
        _stocks.TryGetValue(code.Trim().ToUpperInvariant(), out var stock);
        return Task.FromResult(stock);
    }

    public Task<IReadOnlyCollection<Stock>> ListAsync(CancellationToken ct)
        => Task.FromResult((IReadOnlyCollection<Stock>)_stocks.Values.OrderBy(x => x.Code).ToArray());
}
