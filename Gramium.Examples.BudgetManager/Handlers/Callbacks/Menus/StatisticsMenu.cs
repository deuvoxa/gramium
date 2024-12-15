using Gramium.Core.Entities.Messages;
using Gramium.Examples.BudgetManager.Entities;
using Gramium.Examples.BudgetManager.Services;
using Gramium.Framework.Callbacks;
using Gramium.Framework.Context.Interfaces;
using Gramium.Framework.Extensions;

namespace Gramium.Examples.BudgetManager.Handlers.Callbacks.Menus;

public class StatisticsMenu : CallbackQueryBase
{
    public override string CallbackData => MenuButtons.StatisticsMenu.Item2;

    public override async Task HandleAsync(ICallbackQueryContext context, CancellationToken ct = default)
    {
        var userService = context.Services.GetRequiredService<UserService>();
        var user = await userService.GetUserByTelegramIdAsync(context.CallbackQuery.From.Id);

        var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var endOfToday = DateTime.Now;

        var totalIncome = user.Transactions
            .Where(t => t.Date >= startOfMonth && t.Date <= endOfToday && t.Type == TransactionType.Income)
            .Sum(t => t.Amount);

        var totalExpenses = user.Transactions
            .Where(t => t.Date >= startOfMonth && t.Date <= endOfToday && t.Type == TransactionType.Expense)
            .Sum(t => t.Amount);

        var text = $"""
                    *Меню статистики:*

                    Доход за текущий месяц: {totalIncome} ₽
                    Расход за текущий месяц: {totalExpenses} ₽
                    """;

        var keyboard = context.CreateKeyboard();

        if (totalIncome > 0) keyboard.WithButton(StatisticButtons.Get.Incomes);
        if (totalExpenses > 0) keyboard.WithButton(StatisticButtons.Get.Expenses);

        keyboard.WithButtons(MenuButtons.HomeMenu);

        await context.EditTextMessageAsync(text, ParseMode.MarkdownV2, keyboard.Build());
    }
}