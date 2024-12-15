namespace Gramium.Core.Exceptions;

public class TelegramApiException : GramiumException
{
    public TelegramApiException(string message, int errorCode)
        : base(message)
    {
        ErrorCode = errorCode;
    }

    public TelegramApiException(string message, int errorCode, Exception innerException)
        : base(message, innerException)
    {
        ErrorCode = errorCode;
    }

    public int ErrorCode { get; }
}