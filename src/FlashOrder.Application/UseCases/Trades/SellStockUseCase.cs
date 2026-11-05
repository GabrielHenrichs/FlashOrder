using FlashOrder.Application.Abstractions;
using FlashOrder.Application.DTOs.Commands;
using FlashOrder.Application.DTOs.Responses;
using FlashOrder.Domain.Entities;
using FlashOrder.Domain.Enums;

namespace FlashOrder.Application.UseCases.Trades;

public sealed class SellStockUseCase(
    IStockRepository stockRepository,
    IPositionRepository positionRepository,
    ITransactionRepository transactionRepository)
{
    public async Task<TransactionResponse> ExecuteAsync(SellStockCommand command, CancellationToken ct)
    {
        if (command.Quantity <= 0)
            throw new Exceptions.ApplicationException("Quantidade deve ser maior que zero.");

        var stock = await stockRepository.GetByCodeAsync(command.StockCode, ct)
            ?? throw new Exceptions.ApplicationException("Ação não encontrada.");

        var position = await positionRepository.GetByStockCodeAsync(stock.Code, ct) ?? new Position(stock.Code, 0);

        if (position.Quantity < command.Quantity)
            throw new Exceptions.ApplicationException("Quantidade insuficiente para venda.");

        position.Decrease(command.Quantity);
        await positionRepository.UpsertAsync(position, ct);

        var transaction = Transaction.Create(stock.Code, TransactionType.Sell, command.Quantity, stock.CurrentPrice, DateTime.UtcNow);
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
