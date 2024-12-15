using Gramium.Core.Entities.Messages;
using Gramium.Examples.BudgetManager.Entities;
using Gramium.Examples.BudgetManager.Services;
using Gramium.Framework.Callbacks;
using Gramium.Framework.Context.Interfaces;
using Gramium.Framework.Extensions;

namespace Gramium.Examples.BudgetManager.Handlers.Callbacks.Menus;

public class TransactionsMenu : CallbackQueryBase
{
    public override string CallbackData => MenuButtons.TransactionsMenu.Item2;

    public override async Task HandleAsync(ICallbackQueryContext context, CancellationToken ct = default)
    {
        var userService = context.Services.GetRequiredService<UserService>();
        var user = await userService.GetUserByTelegramIdAsync(context.CallbackQuery.From.Id);

        var transactions = user.Transactions.OrderByDescending(t => t.Date).ToList();
        await context.CreatePagination(transactions)
            .ItemsPerPage(5)
            .FormatItem(t =>
                $"{t.Date:dd.MM.yyy} - {t.Category}: {(t.Type == TransactionType.Income ? "+" : "-")}{t.Amount} (`{t.Id.ToString()[..8]}`)")
            .WithHeader(transactions.Count != 0
                ? "*Все транзакции*\n"
                : "*Все транзакции*\n\nСписок транзакций пуст")
            .WithFooter(transactions.Count == 0 ? "" : "Страница {0}/{1}")
            .NavigationButtons("Вперёд", "Назад")
            .EditAsync(ParseMode.MarkdownV2, TransactionButtons.Add, MenuButtons.HomeMenu);
    }
}