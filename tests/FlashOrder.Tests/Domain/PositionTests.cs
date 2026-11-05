using FlashOrder.Domain.Entities;
using FlashOrder.Domain.Exceptions;

namespace FlashOrder.Tests.Domain;

public class PositionTests
{
    [Fact]
    public void Decrease_WithInsufficientQuantity_ShouldThrowDomainException()
    {
        var position = new Position("PETR4", 5);

        var ex = Assert.Throws<DomainException>(() => position.Decrease(6));

        Assert.Contains("insuficiente", ex.Message, StringComparison.OrdinalIgnoreCase);
    }
}
