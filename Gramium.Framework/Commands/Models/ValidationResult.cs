namespace Gramium.Framework.Commands.Models;

public class ValidationResult
{
    private ValidationResult(bool isValid, string? errorMessage = null)
    {
        IsValid = isValid;
        ErrorMessage = errorMessage;
    }

    public bool IsValid { get; }
    public string? ErrorMessage { get; }

    public static ValidationResult Success()
    {
        return new ValidationResult(true);
    }

    public static ValidationResult Error(string message)
    {
        return new ValidationResult(false, message);
    }
}