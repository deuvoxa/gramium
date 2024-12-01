namespace Gramium.Core.Exceptions;

public class GramiumException : Exception
{
    public GramiumException(string message) : base(message)
    {
    }

    public GramiumException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}