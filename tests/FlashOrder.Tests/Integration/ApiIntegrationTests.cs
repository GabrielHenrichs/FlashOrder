using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace FlashOrder.Tests.Integration;

public class ApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    private static string NewCode(string prefix)
        => $"{prefix}{Guid.NewGuid():N}"[..10].ToUpperInvariant();

    [Fact]
    public async Task PostStock_WithValidRequest_ShouldReturnCreated()
    {
        var code = NewCode("PETR");

        var response = await _client.PostAsJsonAsync("/api/v1/stocks", new
        {
            code,
            companyName = "Petrobras",
            currentPrice = 30m
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task PostStock_WithDuplicateCode_ShouldReturnConflict()
    {
        var code = NewCode("VALE");

        await _client.PostAsJsonAsync("/api/v1/stocks", new
        {
            code,
            companyName = "Vale",
            currentPrice = 60m
        });

        var response = await _client.PostAsJsonAsync("/api/v1/stocks", new
        {
            code,
            companyName = "Vale SA",
            currentPrice = 61m
        });

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task PostStock_WithInvalidPrice_ShouldReturnUnprocessableEntity()
    {
        var code = NewCode("BBDC");

        var response = await _client.PostAsJsonAsync("/api/v1/stocks", new
        {
            code,
            companyName = "Bradesco",
            currentPrice = 0m
        });

        Assert.Equal((HttpStatusCode)422, response.StatusCode);
    }

    [Fact]
    public async Task GetStocks_ShouldReturnRegisteredStocks()
    {
        var code = NewCode("ABEV");

        await _client.PostAsJsonAsync("/api/v1/stocks", new
        {
            code,
            companyName = "Ambev",
            currentPrice = 12m
        });

        var response = await _client.GetAsync("/api/v1/stocks");
        var payload = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains(code, payload, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task PostBuy_WithUnknownStock_ShouldReturnNotFound()
    {
        var unknownCode = NewCode("MISS");

        var response = await _client.PostAsJsonAsync("/api/v1/trades/buy", new
        {
            stockCode = unknownCode,
            quantity = 1
        });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task PostBuy_WithInvalidQuantity_ShouldReturnUnprocessableEntity()
    {
        var code = NewCode("MGLU");

        await _client.PostAsJsonAsync("/api/v1/stocks", new
        {
            code,
            companyName = "Magalu",
            currentPrice = 3m
        });

        var response = await _client.PostAsJsonAsync("/api/v1/trades/buy", new
        {
            stockCode = code,
            quantity = 0
        });

        Assert.Equal((HttpStatusCode)422, response.StatusCode);
    }

    [Fact]
    public async Task PostSell_WithUnknownStock_ShouldReturnNotFound()
    {
        var unknownCode = NewCode("MISS");

        var response = await _client.PostAsJsonAsync("/api/v1/trades/sell", new
        {
            stockCode = unknownCode,
            quantity = 1
        });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task PostSell_WithInvalidQuantity_ShouldReturnUnprocessableEntity()
    {
        var code = NewCode("USIM");

        await _client.PostAsJsonAsync("/api/v1/stocks", new
        {
            code,
            companyName = "Usiminas",
            currentPrice = 8m
        });

        var response = await _client.PostAsJsonAsync("/api/v1/trades/sell", new
        {
            stockCode = code,
            quantity = 0
        });

        Assert.Equal((HttpStatusCode)422, response.StatusCode);
    }

    [Fact]
    public async Task PostSell_WithInsufficientPosition_ShouldReturnUnprocessableEntity()
    {
        var code = NewCode("ITUB");

        await _client.PostAsJsonAsync("/api/v1/stocks", new
        {
            code,
            companyName = "Itau",
            currentPrice = 25m
        });

        await _client.PostAsJsonAsync("/api/v1/trades/buy", new
        {
            stockCode = code,
            quantity = 1
        });

        var response = await _client.PostAsJsonAsync("/api/v1/trades/sell", new
        {
            stockCode = code,
            quantity = 2
        });

        Assert.Equal((HttpStatusCode)422, response.StatusCode);
    }

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
}
