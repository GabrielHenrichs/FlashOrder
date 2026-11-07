using FlashOrder.Domain.Entities;
using FlashOrder.Domain.Exceptions;

namespace FlashOrder.Tests.Domain;

public class StockTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateStock()
    {
        var stock = Stock.Create("ITUB4", "Itaú Unibanco", 40.53m);

        Assert.Equal("ITUB4", stock.Code);
        Assert.Equal("Itaú Unibanco", stock.CompanyName);
        Assert.Equal(40.53m, stock.CurrentPrice);
    }

    [Fact]
    public void Create_WithEmptyCode_ShouldThrowDomainException()
    {
        var ex = Assert.Throws<DomainException>(() => Stock.Create("", "Itaú Unibanco", 40.53m));
        Assert.Contains("Código", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Create_WithEmptyCompanyName_ShouldThrowDomainException()
    {
        var ex = Assert.Throws<DomainException>(() => Stock.Create("ITUB4", "", 40.53m));
        Assert.Contains("Nome", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Create_WithNonPositivePrice_ShouldThrowDomainException()
    {
        var ex = Assert.Throws<DomainException>(() => Stock.Create("ITUB4", "Itaú Unibanco", 0m));
        Assert.Contains("Preço", ex.Message, StringComparison.OrdinalIgnoreCase);
    }
}
