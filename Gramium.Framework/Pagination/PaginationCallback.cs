using Gramium.Framework.Callbacks;
using Gramium.Framework.Context.Interfaces;
using Gramium.Framework.Database.Services;
using Gramium.Framework.Markup;

namespace Gramium.Framework.Pagination;

public class PaginationCallback(IPayloadService payloadService)
    : PayloadCallbackBase<PaginationPayload>
{
    public override string CallbackData => "pagination";

    protected override async Task HandlePayloadAsync(
        ICallbackQueryPayloadContext<PaginationPayload> context,
        CancellationToken ct = default)
    {
        var payload = context.Payload;
        var newPage = payload.Direction switch
        {
            "next" when payload.Page < payload.TotalPages => payload.Page + 1,
            "previous" when payload.Page > 1 => payload.Page - 1,
            _ => payload.Page
        };

        if (newPage == payload.Page) return;

        var pagedItems = payload.Items
            .Skip((newPage - 1) * payload.ItemsPerPage)
            .Take(payload.ItemsPerPage)
            .Select(x => x.Formatted);

        var messageText = string.Join("\n", new[]
        {
            payload.Header,
            string.Join("\n", pagedItems),
            payload.Footer != null ? string.Format(payload.Footer, newPage, payload.TotalPages) : null
        }.Where(x => !string.IsNullOrEmpty(x)));

        var keyboard = new InlineKeyboardBuilder();

        if (newPage > 1)
            keyboard.WithButton((payload.PreviousButtonText,
                await payloadService.SavePayloadAsync(GetType().FullName!,
                    payload with { Page = newPage, Direction = "previous" })));

        if (newPage < payload.TotalPages)
            keyboard.WithButton((payload.NextButtonText,
                await payloadService.SavePayloadAsync(GetType().FullName!,
                    payload with { Page = newPage, Direction = "next" })));

        foreach (var button in payload.AdditionalButtons) keyboard.WithButton(button);

        await context.EditTextMessageAsync(messageText, replyMarkup: keyboard.Build());
    }
}