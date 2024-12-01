namespace Gramium.Core.Exceptions;

public class TelegramApiException : GramiumException
{
    public int ErrorCode { get; }

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
}