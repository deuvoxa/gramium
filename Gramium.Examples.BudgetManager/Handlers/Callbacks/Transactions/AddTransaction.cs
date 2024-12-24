using Gramium.Core.Entities.Messages;
using Gramium.Examples.BudgetManager.Entities;
using Gramium.Examples.BudgetManager.Extensions;
using Gramium.Examples.BudgetManager.Handlers.Commands;
using Gramium.Framework.Callbacks;
using Gramium.Framework.Context.Interfaces;
using Gramium.Framework.Extensions;
using Gramium.Framework.States;

namespace Gramium.Examples.BudgetManager.Handlers.Callbacks.Transactions;

public class AddTransaction : CallbackQueryBase
{
    public override string CallbackData => TransactionButtons.Add.Item2;

    public override async Task HandleAsync(ICallbackQueryContext context, CancellationToken ct = default)
    {
        var user = await context.GetUserAsync();
        var accounts = user.GetActiveAccount();
        if (accounts.Count == 0)
        {
            var addAccountKeyboard = context.CreateKeyboard()
                .WithButtons(AccountButtons.Add)
                .WithButtons(MenuButtons.HomeMenu)
                .Build();
            await context.EditTextMessageAsync(
                @"У Вас нет счетов\! Для добавления новой транзакции добавьте новый счёт\.",
                ParseMode.MarkdownV2, addAccountKeyboard);
            return;
        }

        var keyboard = context.CreateKeyboard();

        foreach (var account in accounts) keyboard.WithButtonRow(account.Name, account.Id.ToString());

        await context.EditTextMessageAsync("Выберите счёт транзакции:", ParseMode.MarkdownV2, keyboard.Build());

        await context.SetStateAsync<TransactionState, AddTransactionStateHandler>(
            context.CallbackQuery.From.Id,
            new TransactionState { Step = "account" });
    }
}

public class AddTransactionStateHandler : BaseStateHandler<TransactionState>
{
    public override string StateKey => "transaction-add";

    public override async Task<bool> HandleMessageAsync(IMessageContext context, TransactionState state)
    {
        switch (state.Step)
        {
            case "amount":
            {
                if (context.Message.Text == "/start") return false;
                var mainMessageId = await context.GetMetadataAsync("MainMessageId");
                // todo: протестить
                if (mainMessageId is null)
                {
                    await new StartCommand().HandleAsync(context);
                    mainMessageId = await context.GetMetadataAsync("MainMessageId");
                }

                if (!decimal.TryParse(context.Message.Text, out var amount))
                {
                    await context.EditTextMessageAsync(long.Parse(mainMessageId!),
                        "Введите введите корректную сумму транзакции:");
                    return false;
                }

                state.Amount = amount;
                state.Step = "confirm";

                var user = await context.GetUserAsync();
                var account = user.Accounts.Single(a => a.Id.ToString() == state.AccountId);
                var isIncomeText = state.IsIncome ? "доход" : "расход";
                var text = $"Добавляю следующий {isIncomeText}?\n\n" +
                           $"Счёт списания: `{account.Name}`\n" +
                           $"Категория: `{state.Category}`\n" +
                           $"Сумма: `{state.Amount}`";

                var keyboard = context.CreateKeyboard()
                    .WithButtons(("Добавить описание", "add-description"))
                    .WithButtons(("Изменить дату", "change-date"))
                    .WithButtons(("Да", "yea-confirm"), ("Нет", "no-confirm"))
                    .Build();

                await context.EditTextMessageAsync(long.Parse(mainMessageId!), text, ParseMode.MarkdownV2, keyboard);

                await context.DeleteMessageAsync(context.Message.MessageId);

                await context.SetStateAsync<TransactionState, AddTransactionStateHandler>(
                    context.Message.From!.Id, state);

                return true;
            }
        }

        return false;
    }

    public override async Task<bool> HandleCallbackQueryAsync(ICallbackQueryContext context, TransactionState state)
    {
        if (context.CallbackQuery.Data == MenuButtons.TransactionsMenu.Item2) return false;
        switch (state.Step)
        {
            case "account":
            {
                // var user = await context.GetUserAsync();
                // var account = user.Accounts.Single(a => a.Id.ToString() == context.CallbackQuery.Data);

                state.Step = "category";
                state.AccountId = context.CallbackQuery.Data;

                List<(string, string)> categories =
                [
                    ("Продукты", $"{TransactionButtons.SelectCategory}Еда"),
                    ("Транспорт", $"{TransactionButtons.SelectCategory}Транспорт"),
                    ("Зарплата", $"{TransactionButtons.SelectCategory}Зарплата"),
                    ("Другое", $"{TransactionButtons.SelectCategory}Другое"),
                    ("Жильё", $"{TransactionButtons.SelectCategory}Жильё")
                ];

                // todo: свои категории
                // categories.AddRange(user.Metadata.Where(m => m.Attribute is "Category")
                //     .Select(category => (category.Value, $"{TransactionButtons.SelectCategory}{category.Value}")));

                var keyboard = context.CreateKeyboard()
                    .WithButtonGrid(categories)
                    .WithButtons(MenuButtons.TransactionsMenu)
                    .Build();

                await context.EditTextMessageAsync("Выберите категорию транзакции:", ParseMode.MarkdownV2, keyboard);

                await context.SetStateAsync<TransactionState, AddTransactionStateHandler>(
                    context.CallbackQuery.From.Id, state);

                return true;
            }
            case "category":
            {
                var category = context.CallbackQuery.Data!.Replace(TransactionButtons.SelectCategory, string.Empty);

                state.Step = "isIncome";
                state.Category = category;

                var keyboard = context.CreateKeyboard()
                    .WithButtons(("Доход", "income"), ("Расход", "expense"))
                    .Build();

                await context.EditTextMessageAsync("Это расход, или доход?", ParseMode.MarkdownV2, keyboard);

                await context.SetStateAsync<TransactionState, AddTransactionStateHandler>(
                    context.CallbackQuery.From.Id, state);

                return true;
            }
            case "isIncome":
            {
                state.Step = "amount";
                state.IsIncome = context.CallbackQuery.Data! == "income";

                await context.EditTextMessageAsync("Введите сумму транзакции:", ParseMode.MarkdownV2);

                await context.SetStateAsync<TransactionState, AddTransactionStateHandler>(
                    context.CallbackQuery.From.Id, state);

                return true;
            }
            case "confirm":
            {
                switch (context.CallbackQuery.Data)
                {
                    case "yea-confirm":
                    {
                        var keyboard = context.CreateKeyboard()
                            .WithButtons(MenuButtons.TransactionsMenu)
                            .Build();
                        await context.EditTextMessageAsync(@"Транзакция успешно сохранена\.", ParseMode.MarkdownV2, keyboard);
                        await context.RemoveStateAsync<AddTransactionStateHandler>(context.CallbackQuery.From.Id);
                        return true;
                    }
                    case "no-confirm":
                    {
                        var keyboard = context.CreateKeyboard()
                            .WithButtons(MenuButtons.TransactionsMenu)
                            .Build();
                        await context.EditTextMessageAsync(@"Транзакция не была сохранена\.", ParseMode.MarkdownV2, keyboard);
                        await context.RemoveStateAsync<AddTransactionStateHandler>(context.CallbackQuery.From.Id);
                        return true;
                    }
                    case "add-description":
                    {
                        return true;
                    }
                    case "change-date":
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        return false;
    }
}

public class TransactionState
{
    public string Step { get; set; } = null!;
    public string? AccountId { get; set; }
    public string? Category { get; set; }
    public bool IsIncome { get; set; }
    public decimal? Amount { get; set; }
    public string? Description { get; set; }
    public DateTime? Date { get; set; }
}