using Gramium.Core.Entities.Messages;
using Gramium.Framework.Commands;
using Gramium.Framework.Context.Interfaces;

namespace Gramium.Examples.Basic.Handlers;

public class TestCommand : CommandBase
{
    public override string Command => "/test";
    public override async Task HandleAsync(IMessageContext context, CancellationToken ct = default)
    {
        await context.SendMessageAsync("_*Тестовое сообщение*_", ParseMode.MarkdownV2);
    }
}