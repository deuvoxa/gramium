namespace Gramium.Framework.Pagination;

public record PaginationPayload
{
    public required int Page { get; init; }
    public required int TotalPages { get; init; }
    public required int ItemsPerPage { get; init; }
    public required IReadOnlyList<ItemWrapper> Items { get; init; }
    public required string? Header { get; init; }
    public required string? Footer { get; init; }
    public required string PreviousButtonText { get; init; }
    public required string NextButtonText { get; init; }
    public required List<(string Text, string CallbackData)> AdditionalButtons { get; init; } = [];
    public string Direction { get; init; } = "";
}

public class ItemWrapper(object original, string formatted)
{
    public object Original { get; } = original;
    public string Formatted { get; } = formatted;
}