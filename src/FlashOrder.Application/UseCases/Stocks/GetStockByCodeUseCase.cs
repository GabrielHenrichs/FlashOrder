using FlashOrder.Application.Abstractions;
using FlashOrder.Application.DTOs.Responses;
using FlashOrder.Application.Exceptions;
using ApplicationLayerException = FlashOrder.Application.Exceptions.ApplicationException;

namespace FlashOrder.Application.UseCases.Stocks;

public sealed class GetStockByCodeUseCase(IStockRepository stockRepository) : IGetStockByCodeUseCase
{
    public async Task<StockResponse> ExecuteAsync(string code, CancellationToken ct)
    {
        var stock = await stockRepository.GetByCodeAsync(code, ct)
            ?? throw new ApplicationLayerException(ApplicationErrorCode.NotFound, "Ação não encontrada.");

        return new StockResponse(stock.Code, stock.CompanyName, stock.CurrentPrice);
    }
}
