using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace FlashOrder.Tests.Integration;

public class TradesApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public TradesApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    private static string NewCode(string prefix)
        => $"{prefix}{Guid.NewGuid():N}"[..10].ToUpperInvariant();

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
}
