using FlashOrder.Domain.Entities;
using FlashOrder.Domain.Exceptions;

namespace FlashOrder.UnitTests.Domain;

public class PositionTests
{
    [Fact]
    public void Constructor_WithValidData_ShouldCreatePosition()
    {
        var position = new Position("ITUB4", 100);

        Assert.Equal("ITUB4", position.StockCode);
        Assert.Equal(100, position.Quantity);
    }

    [Fact]
    public void Constructor_WithEmptyCode_ShouldThrowDomainException()
    {
        var ex = Assert.Throws<DomainException>(() => new Position("", 5));

        Assert.Contains("C\\u00f3digo", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Constructor_WithNegativeQuantity_ShouldThrowDomainException()
    {
        var ex = Assert.Throws<DomainException>(() => new Position("PETR4", -1));

        Assert.Contains("negativa", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Increase_WithNonPositiveQuantity_ShouldThrowDomainException()
    {
        var position = new Position("PETR4", 5);

        var ex = Assert.Throws<DomainException>(() => position.Increase(0));

        Assert.Contains("maior que zero", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Decrease_WithNonPositiveQuantity_ShouldThrowDomainException()
    {
        var position = new Position("PETR4", 5);

        var ex = Assert.Throws<DomainException>(() => position.Decrease(0));

        Assert.Contains("maior que zero", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Decrease_WithInsufficientQuantity_ShouldThrowDomainException()
    {
        var position = new Position("PETR4", 5);

        var ex = Assert.Throws<DomainException>(() => position.Decrease(6));

        Assert.Contains("insuficiente", ex.Message, StringComparison.OrdinalIgnoreCase);
    }
}
