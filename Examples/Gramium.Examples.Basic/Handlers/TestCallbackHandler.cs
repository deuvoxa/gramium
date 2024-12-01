using Gramium.Framework.Context;
using Gramium.Framework.Interfaces;

namespace Gramium.Examples.Basic.Handlers;

public class TestCallbackHandler : ICallbackQueryHandler
{
    public string CallbackData => "test";

    public async Task HandleAsync(ICallbackQueryContext context, CancellationToken ct = default)
    {
        await context.AnswerCallbackQueryAsync("Кнопка нажата!");
        await context.EditMessageTextAsync("Текст сообщения обновлен");
    }
}