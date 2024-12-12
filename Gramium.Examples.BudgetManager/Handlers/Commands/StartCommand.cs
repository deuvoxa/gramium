using Gramium.Core.Entities.Messages;
using Gramium.Examples.BudgetManager.Entities;
using Gramium.Examples.BudgetManager.Services;
using Gramium.Framework.Commands;
using Gramium.Framework.Context.Interfaces;
using Gramium.Framework.Extensions;

namespace Gramium.Examples.BudgetManager.Handlers.Commands;

public class StartCommand : CommandBase
{
    public override string Command => "/start";

    public override async Task HandleAsync(IMessageContext context, CancellationToken ct = default)
    {
        await GetOrCreateUser(context);
        const string text = "_*Главное меню:*_";
        var keyboard = context.CreateKeyboard()
            .AddButtons(Buttons.TransactionsMenu, Buttons.AccountsMenu)
            .AddButtons(Buttons.HomeMenu)
            .Build();

        await context.SendMessageAsync(text, ParseMode.MarkdownV2, keyboard);
    }

    private static async Task<User> GetOrCreateUser(IMessageContext context)
    {
        var userService = context.Services.GetRequiredService<UserService>();
        var user = await userService.GetUserByTelegramIdAsync(context.Message.From!.Id);

        if (user is not null) return user;
        user = new User
        {
            TelegramId = context.Message.From!.Id,
            FirstName = context.Message.From.FirstName,
        };
        await userService.AddUserAsync(user);
        return user;
    }
}