using FlashOrder.Application.Exceptions;
using ApplicationLayerException = FlashOrder.Application.Exceptions.ApplicationException;

namespace FlashOrder.UnitTests.Application.Exceptions;

public class ApplicationExceptionTests
{
    [Fact]
    public void Constructor_ShouldStoreErrorCode()
    {
        var exception = new ApplicationLayerException(ApplicationErrorCode.NotFound, "not found");

        Assert.Equal(ApplicationErrorCode.NotFound, exception.ErrorCode);
        Assert.Equal("not found", exception.Message);
    }
}
