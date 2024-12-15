using Gramium.Core.Entities.Messages;
using Gramium.Framework.Context.Interfaces;
using Gramium.Framework.Extensions;
using Gramium.Framework.Markup;

namespace Gramium.Framework.Pagination;

public class PaginationBuilder<T>(IBaseContext context, IEnumerable<T> items)
    where T : class
{
    private string? _footer;
    private Func<T, string?> _formatItem = item => item.ToString();
    private string? _header;
    private int _itemsPerPage = 10;
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

    public async Task SendAsync(ParseMode parseMode = ParseMode.None,
        params (string Text, string CallbackData)[] additionalButtons)
    {
        var itemsList = items.ToList();
        var totalItems = itemsList.Count;
        var totalPages = (int)Math.Ceiling(totalItems / (double)_itemsPerPage);
        const int currentPage = 1;

        await SendPageAsync(currentPage, totalPages, parseMode, additionalButtons);
    }

    public async Task EditAsync(ParseMode parseMode = ParseMode.None,
        params (string Text, string CallbackData)[] additionalButtons)
    {
        var itemsList = items.ToList();
        var totalItems = itemsList.Count;
        var totalPages = (int)Math.Ceiling(totalItems / (double)_itemsPerPage);
        const int currentPage = 1;

        await EditPageAsync(currentPage, totalPages, parseMode, additionalButtons);
    }

    private async Task SendPageAsync(int page, int totalPages, ParseMode parseMode,
        params (string Text, string CallbackData)[] additionalButtons)
    {
        var keyboard = new InlineKeyboardBuilder();

        if (page > 1) keyboard.WithPayloadButton(context, _previousButtonText, CreatePayload("previous"));

        if (page < totalPages) keyboard.WithPayloadButton(context, _nextButtonText, CreatePayload("next"));

        foreach (var button in additionalButtons) keyboard.WithButton(button);

        var start = (page - 1) * _itemsPerPage;
        var pagedItems = items.Skip(start).Take(_itemsPerPage);
        var formattedItems = string.Join("\n", pagedItems.Select(_formatItem));

        var messageText = string.Join("\n", new[]
        {
            _header,
            formattedItems,
            _footer != null ? string.Format(_footer, page, totalPages) : null
        }.Where(x => !string.IsNullOrEmpty(x)));

        await context.SendMessageAsync(messageText, parseMode, keyboard.Build());
        return;

        PaginationPayload CreatePayload(string direction)
        {
            return new PaginationPayload
            {
                Page = page,
                TotalPages = totalPages,
                ItemsPerPage = _itemsPerPage,
                Items = items.Select(x => new ItemWrapper(x, _formatItem(x)!)).ToList(),
                Direction = direction,
                Header = _header,
                Footer = _footer,
                NextButtonText = _nextButtonText,
                AdditionalButtons = additionalButtons.ToList(),
                PreviousButtonText = _previousButtonText
            };
        }
    }

    private async Task EditPageAsync(int page, int totalPages, ParseMode parseMode,
        params (string Text, string CallbackData)[] additionalButtons)
    {
        var keyboard = new InlineKeyboardBuilder();

        if (page > 1) keyboard.WithPayloadButton(context, _previousButtonText, CreatePayload("previous"));

        if (page < totalPages) keyboard.WithPayloadButton(context, _nextButtonText, CreatePayload("next"));

        foreach (var button in additionalButtons) keyboard.WithButton(button);

        var start = (page - 1) * _itemsPerPage;
        var pagedItems = items.Skip(start).Take(_itemsPerPage);
        var formattedItems = string.Join("\n", pagedItems.Select(_formatItem));

        var messageText = string.Join("\n", new[]
        {
            _header,
            formattedItems,
            _footer != null ? string.Format(_footer, page, totalPages) : null
        }.Where(x => !string.IsNullOrEmpty(x)));

        // TODO: возможна ошибка если context is IMessageContext
        if (context is ICallbackQueryContext callbackContext)
            await callbackContext.EditTextMessageAsync(messageText, parseMode, keyboard.Build());
        return;

        PaginationPayload CreatePayload(string direction)
        {
            return new PaginationPayload
            {
                Page = page,
                TotalPages = totalPages,
                ItemsPerPage = _itemsPerPage,
                Items = items.Select(x => new ItemWrapper(x, _formatItem(x)!)).ToList(),
                Direction = direction,
                Header = _header,
                Footer = _footer,
                NextButtonText = _nextButtonText,
                AdditionalButtons = additionalButtons.ToList(),
                PreviousButtonText = _previousButtonText
            };
        }
    }
}