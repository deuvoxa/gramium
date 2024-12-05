using Gramium.Framework.Commands;
using Gramium.Framework.Commands.Models;
using Gramium.Framework.Context;
using Gramium.Framework.Markup;

namespace Gramium.Examples.Basic.Handlers;

public class BanCommand : CommandBase
{
    public override string Command => "/ban";
    public override string Description => "Забанить пользователя";

    public override CommandParameter[] Parameters =>
    [
        new("username", typeof(string), description: "Имя пользователя"),
        new("duration", typeof(int), true, 24, "Длительность бана в часах")
    ];

    public override async Task HandleAsync(IMessageContext context, CancellationToken ct = default)
    {
        var args = context.Message.Text!.Split(' ').Skip(1).ToArray();
        var validation = Validate(args);

        if (!validation.IsValid)
        {
            await context.ReplyAsync(validation.ErrorMessage!);
            return;
        }

        var parameters = ParseParameters(args);
        var username = (string)parameters[0]!;
        var duration = (int?)parameters[1] ?? 24;

        var keyboard = context.CreateKeyboard()
            .AddButton("Menu", "menu")
            .Build();

        await context.ReplyAsync($"Пользователь {username} забанен на {duration} часов", keyboard);
    }
}