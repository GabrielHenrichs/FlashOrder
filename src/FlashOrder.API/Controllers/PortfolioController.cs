using FlashOrder.Application.UseCases.Portfolio;
using Microsoft.AspNetCore.Mvc;

namespace FlashOrder.API.Controllers;

[ApiController]
[Route("api/portfolio")]
public sealed class PortfolioController(
    IGetPositionsUseCase getPositionsUseCase,
    IGetTransactionsUseCase getTransactionsUseCase) : ControllerBase
{
    [HttpGet("positions")]
    public async Task<IActionResult> GetPositions(CancellationToken ct)
    {
        var result = await getPositionsUseCase.ExecuteAsync(ct);
        return Ok(result);
    }

    [HttpGet("transactions")]
    public async Task<IActionResult> GetTransactions(CancellationToken ct)
    {
        var result = await getTransactionsUseCase.ExecuteAsync(ct);
        return Ok(result);
    }
}
