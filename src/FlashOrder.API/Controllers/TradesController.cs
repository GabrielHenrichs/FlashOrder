using FlashOrder.API.Contracts.Requests;
using FlashOrder.Application.DTOs.Commands;
using FlashOrder.Application.UseCases.Trades;
using Microsoft.AspNetCore.Mvc;

namespace FlashOrder.API.Controllers;

[ApiController]
[Route("api/trades")]
public sealed class TradesController(
    IBuyStockUseCase buyStockUseCase,
    ISellStockUseCase sellStockUseCase) : ControllerBase
{
    [HttpPost("buy")]
    public async Task<IActionResult> Buy([FromBody] TradeRequest request, CancellationToken ct)
    {
        var result = await buyStockUseCase.ExecuteAsync(
            new BuyStockCommand(request.StockCode, request.Quantity),
            ct);

        return Ok(result);
    }

    [HttpPost("sell")]
    public async Task<IActionResult> Sell([FromBody] TradeRequest request, CancellationToken ct)
    {
        var result = await sellStockUseCase.ExecuteAsync(
            new SellStockCommand(request.StockCode, request.Quantity),
            ct);

        return Ok(result);
    }
}
