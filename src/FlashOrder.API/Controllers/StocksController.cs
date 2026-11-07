using FlashOrder.API.Contracts.Requests;
using FlashOrder.Application.DTOs.Commands;
using FlashOrder.Application.UseCases.Stocks;
using Microsoft.AspNetCore.Mvc;

namespace FlashOrder.API.Controllers;

[ApiController]
[Route("api/stocks")]
public sealed class StocksController(
    ICreateStockUseCase createStockUseCase,
    IGetStocksUseCase getStocksUseCase) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockRequest request, CancellationToken ct)
    {
        var result = await createStockUseCase.ExecuteAsync(
            new CreateStockCommand(request.Code, request.CompanyName, request.CurrentPrice),
            ct);

        return Created($"/api/stocks/{result.Code}", result);
    }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var result = await getStocksUseCase.ExecuteAsync(ct);
        return Ok(result);
    }
}
