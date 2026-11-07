using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace FlashOrder.Tests.API;

public class ApiContractTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ApiContractTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostStock_WithValidRequest_ShouldReturnCreated()
    {
        var response = await _client.PostAsJsonAsync("/api/stocks", new
        {
            code = "PETR4",
            companyName = "Petrobras",
            currentPrice = 30m
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task PostStock_WithDuplicateCode_ShouldReturnConflict()
    {
        await _client.PostAsJsonAsync("/api/stocks", new
        {
            code = "VALE3",
            companyName = "Vale",
            currentPrice = 60m
        });

        var response = await _client.PostAsJsonAsync("/api/stocks", new
        {
            code = "VALE3",
            companyName = "Vale SA",
            currentPrice = 61m
        });

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task PostSell_WithInsufficientPosition_ShouldReturnUnprocessableEntity()
    {
        await _client.PostAsJsonAsync("/api/stocks", new
        {
            code = "ITUB4",
            companyName = "Itau",
            currentPrice = 25m
        });

        await _client.PostAsJsonAsync("/api/trades/buy", new
        {
            stockCode = "ITUB4",
            quantity = 1
        });

        var response = await _client.PostAsJsonAsync("/api/trades/sell", new
        {
            stockCode = "ITUB4",
            quantity = 2
        });

        Assert.Equal((HttpStatusCode)422, response.StatusCode);
    }
}
