using FlashOrder.Application.Abstractions;
using FlashOrder.Application.DTOs.Responses;

namespace FlashOrder.Application.UseCases.Portfolio;

public sealed class GetPositionsUseCase(IPositionRepository positions) : IGetPositionsUseCase
{
    public async Task<IReadOnlyCollection<PositionResponse>> ExecuteAsync(CancellationToken ct)
    {
        var all = await positions.ListAsync(ct);

        return all
            .Where(x => x.Quantity > 0)
            .OrderBy(x => x.StockCode)
            .Select(x => new PositionResponse(x.StockCode, x.Quantity))
            .ToArray();
    }
}
