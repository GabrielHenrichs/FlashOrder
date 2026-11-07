using FlashOrder.Domain.Entities;
using FlashOrder.Domain.Exceptions;

namespace FlashOrder.Tests.Domain;

public class PositionTests
{
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
