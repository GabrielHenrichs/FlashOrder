using FlashOrder.Application.DTOs.Responses;

namespace FlashOrder.Application.UseCases.Portfolio;

public interface IGetPositionsUseCase
{
    Task<IReadOnlyCollection<PositionResponse>> ExecuteAsync(CancellationToken ct);
}
