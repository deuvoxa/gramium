using Gramium.Core.Entities.Messages;
using Gramium.Examples.BudgetManager.Entities;
using Gramium.Examples.BudgetManager.Extensions;
using Gramium.Examples.BudgetManager.Handlers.Callbacks.Transactions;
using Gramium.Framework.Callbacks;
using Gramium.Framework.Context.Interfaces;
using Gramium.Framework.Extensions;
using Gramium.Framework.States;

namespace Gramium.Examples.BudgetManager.Handlers.Callbacks.Accounts;

public class AddAccount : CallbackQueryBase
{
    public override string CallbackData => AccountButtons.Add.Item2;

    public override async Task HandleAsync(ICallbackQueryContext context, CancellationToken ct = default)
    {
        var user = await context.GetUserAsync();
        var accounts = user.GetActiveAccount();
        if (accounts.Count == 0)
        {
        }

        var keyboard = context.CreateKeyboard()
            .WithButtons(MenuButtons.AccountsMenu)
            .Build();


        await context.EditTextMessageAsync("Введите название счёта:", ParseMode.MarkdownV2, keyboard);

        await context.SetStateAsync<AccountsState, AddAccountStateHandler>(
            context.CallbackQuery.From.Id,
            new AccountsState { Step = "account-name" });
    }
}

public class AddAccountStateHandler : BaseStateHandler<AccountsState>
{
    public override string StateKey => "account-add";

    public override async Task<bool> HandleMessageAsync(IMessageContext context, AccountsState state)
    {
        if (context.Message.Text == "/start")
        {
            await context.RemoveStateAsync<AddTransactionStateHandler>(context.Message.From!.Id);
            return false;
        }

        var mainMessageId = await context.GetMetadataAsync("MainMessageId");

        switch (state.Step)
        {
            case "account-name":
            {
                state.Step = "account-balance";
                state.AccountName = context.Message.Text;

                await context.EditTextMessageAsync(long.Parse(mainMessageId!), "Введите текущий баланс счёта:");

                await context.DeleteMessageAsync(context.Message.MessageId);

                await context.SetStateAsync<AccountsState, AddAccountStateHandler>(context.Message.From!.Id, state);

                return true;
            }
            case "account-balance":
            {
                if (!decimal.TryParse(context.Message.Text, out var amount))
                {
                    await context.EditTextMessageAsync(long.Parse(mainMessageId!),
                        "Введите введите корректную сумму транзакции:");
                    return true;
                }

                state.AccountBalance = amount;

                var keyboard = context.CreateKeyboard()
                    .WithButtons(MenuButtons.AccountsMenu)
                    .Build();

                await context.EditTextMessageAsync(long.Parse(mainMessageId!),
                    $"Добавил новый счёт `{state.AccountName}`\nТекущий баланс: `{state.AccountBalance}`",
                    ParseMode.MarkdownV2, keyboard);

                await context.DeleteMessageAsync(context.Message.MessageId);

                await context.RemoveStateAsync<AddAccountStateHandler>(context.Message.From!.Id);

                var user = await context.GetUserAsync();

                var account = new Account
                {
                    Name = state.AccountName!,
                    Balance = state.AccountBalance,
                    IsActive = true
                };

                user.Accounts.Add(account);
                await context.UpdateUserAsync(user);

                return true;
            }
        }

        return false;
    }
}

public class AccountsState
{
    public string Step { get; set; } = null!;
    public string? AccountName { get; set; }
    public decimal AccountBalance { get; set; }
}