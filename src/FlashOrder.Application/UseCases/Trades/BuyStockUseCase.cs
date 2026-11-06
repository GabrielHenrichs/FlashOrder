using FlashOrder.Application.Abstractions;
using FlashOrder.Application.DTOs.Commands;
using FlashOrder.Application.DTOs.Responses;
using FlashOrder.Domain.Entities;
using FlashOrder.Domain.Enums;

namespace FlashOrder.Application.UseCases.Trades;

public sealed class BuyStockUseCase(
    IStockRepository stockRepository,
    IPositionRepository positionRepository,
    ITransactionRepository transactionRepository) : IBuyStockUseCase
{
    public async Task<TransactionResponse> ExecuteAsync(BuyStockCommand command, CancellationToken ct)
    {
        if (command.Quantity <= 0)
            throw new Exceptions.ApplicationException("Quantidade deve ser maior que zero.");

        var stock = await stockRepository.GetByCodeAsync(command.StockCode, ct)
            ?? throw new Exceptions.ApplicationException("Acao nao encontrada.");

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
