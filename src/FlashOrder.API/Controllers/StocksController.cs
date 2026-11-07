using FlashOrder.API.Contracts.Requests;
using FlashOrder.Application.DTOs.Commands;
using FlashOrder.Application.DTOs.Responses;
using FlashOrder.Application.UseCases.Stocks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FlashOrder.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/stocks")]
public sealed class StocksController(
    ICreateStockUseCase createStockUseCase,
    IGetStocksUseCase getStocksUseCase) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Cadastra uma acao",
        Description = "Cria uma nova acao com codigo unico, nome da empresa e preco atual.")]
    [ProducesResponseType(typeof(StockResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Contracts.Responses.ErrorResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(Contracts.Responses.ErrorResponse), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(Contracts.Responses.ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateStockRequest request, CancellationToken ct)
    {
        var result = await createStockUseCase.ExecuteAsync(
            new CreateStockCommand(request.Code, request.CompanyName, request.CurrentPrice),
            ct);

        return Created($"/api/v1/stocks/{result.Code}", result);
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Lista acoes cadastradas",
        Description = "Retorna a lista de acoes disponiveis para negociacao.")]
    [ProducesResponseType(typeof(IReadOnlyCollection<StockResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Contracts.Responses.ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var result = await getStocksUseCase.ExecuteAsync(ct);
        return Ok(result);
    }
}
