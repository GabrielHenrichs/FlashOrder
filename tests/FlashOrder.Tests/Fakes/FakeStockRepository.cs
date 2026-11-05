using FlashOrder.Application.Abstractions;
using FlashOrder.Domain.Entities;

namespace FlashOrder.Tests.Fakes;

public sealed class FakeStockRepository : IStockRepository
{
    private readonly Dictionary<string, Stock> _stocks = new(StringComparer.OrdinalIgnoreCase);

    public Task<bool> ExistsAsync(string code, CancellationToken ct)
        => Task.FromResult(_stocks.ContainsKey(code));

    public Task AddAsync(Stock stock, CancellationToken ct)
    {
        _stocks[stock.Code] = stock;
        return Task.CompletedTask;
    }

    public Task<Stock?> GetByCodeAsync(string code, CancellationToken ct)
    {
        _stocks.TryGetValue(code, out var stock);
        return Task.FromResult(stock);
    }

    public Task<IReadOnlyCollection<Stock>> ListAsync(CancellationToken ct)
        => Task.FromResult((IReadOnlyCollection<Stock>)_stocks.Values.ToArray());
}
