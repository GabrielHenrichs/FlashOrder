using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace FlashOrder.IntegrationTests.Controllers;

public class StocksApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public StocksApiIntegrationTests(WebApplicationFactory<Program> factory)
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
    public async Task GetStockByCode_ShouldReturnRegisteredStock()
    {
        var code = NewCode("LREN");

        await _client.PostAsJsonAsync("/api/v1/stocks", new
        {
            code,
            companyName = "Lojas Renner",
            currentPrice = 20m
        });

        var response = await _client.GetAsync($"/api/v1/stocks/{code}");
        var payload = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains(code, payload, StringComparison.OrdinalIgnoreCase);
    }
}
