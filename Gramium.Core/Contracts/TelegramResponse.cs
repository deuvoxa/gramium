namespace Gramium.Core.Contracts;

public class TelegramResponse<T>
{
    public bool Ok { get; init; }
    public T? Result { get; init; }
    public string? Description { get; init; }
    public int? ErrorCode { get; init; }
}