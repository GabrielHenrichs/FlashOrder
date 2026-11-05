using FlashOrder.Domain.Entities;
using FlashOrder.Domain.Exceptions;

namespace FlashOrder.Tests.Domain;

public class StockTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateStock()
    {
        var stock = Stock.Create("PETR4", "Petrobras", 30m);

        Assert.Equal("PETR4", stock.Code);
        Assert.Equal("Petrobras", stock.CompanyName);
        Assert.Equal(30m, stock.CurrentPrice);
    }

    [Fact]
    public void Create_WithNonPositivePrice_ShouldThrowDomainException()
    {
        var ex = Assert.Throws<DomainException>(() => Stock.Create("PETR4", "Petrobras", 0m));

        Assert.Contains("Preço", ex.Message, StringComparison.OrdinalIgnoreCase);
    }
}
