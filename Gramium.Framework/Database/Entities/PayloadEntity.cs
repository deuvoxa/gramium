namespace Gramium.Framework.Database.Entities;

public class PayloadEntity
{
    public string Id { get; init; } = null!;
    public string HandlerType { get; init; } = null!;
    public string JsonData { get; init; } = null!;
}