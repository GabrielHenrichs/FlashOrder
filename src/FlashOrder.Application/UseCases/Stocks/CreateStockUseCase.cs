using FlashOrder.Application.Abstractions;
using FlashOrder.Application.DTOs.Commands;
using FlashOrder.Application.DTOs.Responses;
using FlashOrder.Application.Exceptions;
using FlashOrder.Domain.Entities;
using ApplicationLayerException = FlashOrder.Application.Exceptions.ApplicationException;

namespace FlashOrder.Application.UseCases.Stocks;

public sealed class CreateStockUseCase(IStockRepository stockRepository)
{
    public async Task<StockResponse> ExecuteAsync(CreateStockCommand command, CancellationToken ct)
    {
        var normalizedCode = (command.Code ?? string.Empty).Trim().ToUpperInvariant();

        if (await stockRepository.ExistsAsync(normalizedCode, ct))
            throw new ApplicationLayerException(ApplicationErrorCode.Conflict, "Acao ja cadastrada para o codigo informado.");

        var stock = Stock.Create(normalizedCode, command.CompanyName, command.CurrentPrice);
        await stockRepository.AddAsync(stock, ct);

        return new StockResponse(stock.Code, stock.CompanyName, stock.CurrentPrice);
    }
}
