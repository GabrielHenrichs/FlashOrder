namespace FlashOrder.Application.Exceptions;

public sealed class ApplicationException : Exception
{
    public ApplicationErrorCode ErrorCode { get; }

    public ApplicationException(ApplicationErrorCode errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }
}
