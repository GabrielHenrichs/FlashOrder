using FlashOrder.API.Contracts.Requests;
using FlashOrder.Application.DTOs.Commands;
using FlashOrder.Application.DTOs.Responses;
using FlashOrder.Application.UseCases.Trades;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FlashOrder.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/trades")]
public sealed class TradesController(
    IBuyStockUseCase buyStockUseCase,
    ISellStockUseCase sellStockUseCase) : ControllerBase
{
    [HttpPost("buy")]
    [SwaggerOperation(
        Summary = "Compra acoes",
        Description = "Registra uma operacao de compra e atualiza a posicao do ativo.")]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Contracts.Responses.ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Contracts.Responses.ErrorResponse), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(Contracts.Responses.ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Buy([FromBody] TradeRequest request, CancellationToken ct)
    {
        var result = await buyStockUseCase.ExecuteAsync(
            new BuyStockCommand(request.StockCode, request.Quantity),
            ct);

        return Ok(result);
    }

    [HttpPost("sell")]
    [SwaggerOperation(
        Summary = "Vende acoes",
        Description = "Registra uma operacao de venda e reduz a posicao do ativo.")]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Contracts.Responses.ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Contracts.Responses.ErrorResponse), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(Contracts.Responses.ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Sell([FromBody] TradeRequest request, CancellationToken ct)
    {
        var result = await sellStockUseCase.ExecuteAsync(
            new SellStockCommand(request.StockCode, request.Quantity),
            ct);

        return Ok(result);
    }
}
