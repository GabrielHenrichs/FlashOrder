using FlashOrder.Application.Abstractions;
using FlashOrder.Application.DTOs.Commands;
using FlashOrder.Application.DTOs.Responses;
using FlashOrder.Application.Exceptions;
using FlashOrder.Domain.Entities;
using FlashOrder.Domain.Enums;
using ApplicationLayerException = FlashOrder.Application.Exceptions.ApplicationException;

namespace FlashOrder.Application.UseCases.Trades;

public sealed class BuyStockUseCase(
    IStockRepository stockRepository,
    IPositionRepository positionRepository,
    ITransactionRepository transactionRepository) : IBuyStockUseCase
{
    public async Task<TransactionResponse> ExecuteAsync(BuyStockCommand command, CancellationToken ct)
    {
        if (command.Quantity <= 0)
            throw new ApplicationLayerException(ApplicationErrorCode.Validation, "Quantidade deve ser maior que zero.");

        var stock = await stockRepository.GetByCodeAsync(command.StockCode, ct)
            ?? throw new ApplicationLayerException(ApplicationErrorCode.NotFound, "Acao nao encontrada.");

        var position = await positionRepository.GetByStockCodeAsync(stock.Code, ct) ?? new Position(stock.Code, 0);
        position.Increase(command.Quantity);
        await positionRepository.UpsertAsync(position, ct);

        var transaction = Transaction.Create(stock.Code, TransactionType.Buy, command.Quantity, stock.CurrentPrice, DateTime.UtcNow);
        await transactionRepository.AddAsync(transaction, ct);

        return new TransactionResponse(
            transaction.StockCode,
            transaction.Type.ToString(),
            transaction.Quantity,
            transaction.UnitPrice,
            transaction.TotalValue,
            transaction.OccurredAt);
    }
}
