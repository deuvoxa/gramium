using Gramium.Framework.Commands;
using Gramium.Framework.Commands.Models;
using Gramium.Framework.Context;
using Gramium.Framework.Extensions;

namespace Gramium.Examples.Basic.Handlers;

public class ModerateCommand : CommandBase
{
    public override string Command => "moderate";
    public override string Description => "Модерация пользователя";

    public override CommandParameter[] Parameters =>
    [
        new("user_id", typeof(long), description: "ID пользователя"),
        new CommandParameter("reason", typeof(string), description: "Причина", isOptional: true)
    ];

    public override async Task HandleAsync(IMessageContext context, CancellationToken ct = default)
    {
        var validation = Validate(context.Message.Text!.Split(' '));

        if (!validation.IsValid)
        {
            await context.ReplyAsync(validation.ErrorMessage!);
            return;
        }
        
        var userId = long.Parse(context.Message.Text.Split(' ')[1]);

        var keyboard = context.CreateKeyboard()
            .WithPayloadButton(context, "Бан 30 минут", new ModerateUserPayload 
            { 
                UserId = userId,
                Action = "ban",
                Duration = 30
            })
            .WithPayloadButton(context, "Мут 30 минут", new ModerateUserPayload 
            { 
                UserId = userId,
                Action = "mute",
                Duration = 30
            })
            .Build();

        await context.ReplyAsync(
            $"Выберите действие для пользователя {userId}:", 
            keyboard);
    }
}