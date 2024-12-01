namespace Gramium.Core.Entities.Common;

public class Chat
{
    public long Id { get; set; }
    public string Type { get; set; } = null!;
    public string? Title { get; set; }
    public string? Username { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}