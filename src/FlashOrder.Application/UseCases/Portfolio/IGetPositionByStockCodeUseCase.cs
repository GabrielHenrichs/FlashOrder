using FlashOrder.Application.DTOs.Responses;

namespace FlashOrder.Application.UseCases.Portfolio;

public interface IGetPositionByStockCodeUseCase
{
    Task<PositionResponse> ExecuteAsync(string stockCode, CancellationToken ct);
}
