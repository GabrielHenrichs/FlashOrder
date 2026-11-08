using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace FlashOrder.IntegrationTests.Controllers;

public class PortifolioApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public PortifolioApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    private static string NewCode(string prefix)
        => $"{prefix}{Guid.NewGuid():N}"[..10].ToUpperInvariant();

    [Fact]
    public async Task PortfolioEndpoints_WithBuyAndSell_ShouldReturnExpectedState()
    {
        var code = NewCode("WEGE");

        await _client.PostAsJsonAsync("/api/v1/stocks", new
        {
            code,
            companyName = "Weg",
            currentPrice = 40m
        });

        await _client.PostAsJsonAsync("/api/v1/trades/buy", new
        {
            stockCode = code,
            quantity = 3
        });

        await _client.PostAsJsonAsync("/api/v1/trades/sell", new
        {
            stockCode = code,
            quantity = 1
        });

        var positionsResponse = await _client.GetAsync("/api/v1/portfolio/positions");
        var positionsPayload = await positionsResponse.Content.ReadAsStringAsync();

        var transactionsResponse = await _client.GetAsync("/api/v1/portfolio/transactions");
        var transactionsPayload = await transactionsResponse.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, positionsResponse.StatusCode);
        Assert.Contains(code, positionsPayload, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("\"quantity\":2", positionsPayload, StringComparison.OrdinalIgnoreCase);

        Assert.Equal(HttpStatusCode.OK, transactionsResponse.StatusCode);
        Assert.Contains("Sell", transactionsPayload, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Buy", transactionsPayload, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task GetPositionByStockCode_ShouldReturnExpectedState()
    {
        var code = NewCode("LREN");

        await _client.PostAsJsonAsync("/api/v1/stocks", new
        {
            code,
            companyName = "Lojas Renner",
            currentPrice = 20m
        });

        await _client.PostAsJsonAsync("/api/v1/trades/buy", new
        {
            stockCode = code,
            quantity = 3
        });

        var response = await _client.GetAsync($"/api/v1/portfolio/positions/{code}");
        var payload = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains(code, payload, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task GetTransactionsByStockCode_ShouldReturnRegisteredStock()
    {
        var code = NewCode("LREN");

        await _client.PostAsJsonAsync("/api/v1/stocks", new
        {
            code,
            companyName = "Lojas Renner",
            currentPrice = 20m
        });

        await _client.PostAsJsonAsync("/api/v1/trades/buy", new
        {
            stockCode = code,
            quantity = 3
        });

        var response = await _client.GetAsync($"/api/v1/portfolio/transactions?stockCode={code}");
        var payload = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains(code, payload, StringComparison.OrdinalIgnoreCase);
    }
}
