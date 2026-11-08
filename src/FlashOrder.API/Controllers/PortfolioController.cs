using FlashOrder.Application.UseCases.Portfolio;
using FlashOrder.Application.DTOs.Responses;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FlashOrder.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/portfolio")]
public sealed class PortfolioController(
    IGetPositionsUseCase getPositionsUseCase,
    IGetPositionByStockCodeUseCase getPositionByStockCodeUseCase,
    IGetTransactionsUseCase getTransactionsUseCase) : ControllerBase
{
    [HttpGet("positions")]
    [SwaggerOperation(
        Summary = "Consulta posições atuais",
        Description = "Retorna as quantidades atuais por ativo na carteira.")]
    [ProducesResponseType(typeof(IReadOnlyCollection<PositionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Contracts.Responses.ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPositions(CancellationToken ct)
    {
        var result = await getPositionsUseCase.ExecuteAsync(ct);
        return Ok(result);
    }

    [HttpGet("positions/{stockCode}")]
    [SwaggerOperation(
        Summary = "Consulta posição atual de um ativo",
        Description = "Retorna a quantidade atual de um ativo específico na carteira.")]
    [ProducesResponseType(typeof(PositionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Contracts.Responses.ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Contracts.Responses.ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPositionByStockCode(string stockCode, CancellationToken ct)
    {
        var result = await getPositionByStockCodeUseCase.ExecuteAsync(stockCode, ct);
        return Ok(result);
    }

    [HttpGet("transactions")]
    [SwaggerOperation(
        Summary = "Consulta histórico de transações",
        Description = "Retorna o histórico de compras e vendas concluídas em ordem decrescente de data.")]
    [ProducesResponseType(typeof(IReadOnlyCollection<TransactionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Contracts.Responses.ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTransactions(string? stockCode, CancellationToken ct)
    {
        var result = await getTransactionsUseCase.ExecuteAsync(stockCode, ct);
        return Ok(result);
    }
}
