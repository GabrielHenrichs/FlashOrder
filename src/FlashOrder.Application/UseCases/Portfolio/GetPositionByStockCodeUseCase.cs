using FlashOrder.Application.Abstractions;
using FlashOrder.Application.DTOs.Responses;
using FlashOrder.Application.Exceptions;
using ApplicationLayerException = FlashOrder.Application.Exceptions.ApplicationException;

namespace FlashOrder.Application.UseCases.Portfolio;

public sealed class GetPositionByStockCodeUseCase(IPositionRepository positionRepository) : IGetPositionByStockCodeUseCase
{
    public async Task<PositionResponse> ExecuteAsync(string stockCode, CancellationToken ct)
    {
        var position = await positionRepository.GetByStockCodeAsync(stockCode, ct)
            ?? throw new ApplicationLayerException(ApplicationErrorCode.NotFound, "Posição não encontrada.");

        return new PositionResponse(position.StockCode, position.Quantity);
    }
}
