using Gramium.Framework.Context;
using Gramium.Framework.Extensions;
using Gramium.Framework.Markup;

namespace Gramium.Framework.Pagination;

public class PaginationBuilder<T>(IMessageContext context, IEnumerable<T> items)
    where T : class
{
    private int _itemsPerPage = 10;
    private Func<T, string?> _formatItem = item => item.ToString();
    private string? _header;
    private string? _footer;
    private string _nextButtonText = "Next";
    private string _previousButtonText = "Previous";

    public PaginationBuilder<T> FormatItem(Func<T, string> formatter)
    {
        _formatItem = formatter;
        return this;
    }

    public PaginationBuilder<T> ItemsPerPage(int count)
    {
        _itemsPerPage = count;
        return this;
    }

    public PaginationBuilder<T> WithHeader(string header)
    {
        _header = header;
        return this;
    }

    public PaginationBuilder<T> WithFooter(string footer)
    {
        _footer = footer;
        return this;
    }

    public PaginationBuilder<T> NavigationButtons(string nextText, string previousText)
    {
        _nextButtonText = nextText;
        _previousButtonText = previousText;
        return this;
    }

    public async Task SendAsync()
    {
        var itemsList = items.ToList();
        var totalItems = itemsList.Count;
        var totalPages = (int)Math.Ceiling(totalItems / (double)_itemsPerPage);
        const int currentPage = 1;

        await SendPageAsync(currentPage, totalPages);
    }

    private async Task SendPageAsync(int page, int totalPages)
    {
        var keyboard = new InlineKeyboardBuilder();

        if (page > 1) keyboard.WithPayloadButton(context, _previousButtonText, CreatePayload("previous"));

        if (page < totalPages) keyboard.WithPayloadButton(context, _nextButtonText, CreatePayload("next"));

        var start = (page - 1) * _itemsPerPage;
        var pagedItems = items.Skip(start).Take(_itemsPerPage);
        var formattedItems = string.Join("\n", pagedItems.Select(_formatItem));

        var messageText = string.Join("\n", new[]
        {
            _header,
            formattedItems,
            _footer != null ? string.Format(_footer, page, totalPages) : null
        }.Where(x => !string.IsNullOrEmpty(x)));

        await context.ReplyAsync(messageText, keyboard.Build());
        return;
        
        PaginationPayload CreatePayload(string direction)
            => new()
            {
                Page = page,
                TotalPages = totalPages,
                ItemsPerPage = _itemsPerPage,
                Items = items.Select(x => new ItemWrapper(x, _formatItem(x)!)).ToList(),
                Direction = direction,
                Header = _header,
                Footer = _footer,
                NextButtonText = _nextButtonText,
                PreviousButtonText = _previousButtonText
            };
    }
}

public class ItemWrapper(object original, string formatted)
{
    public object Original { get; } = original;
    public string Formatted { get; } = formatted;
}

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
    public string Direction { get; init; } = "";
}