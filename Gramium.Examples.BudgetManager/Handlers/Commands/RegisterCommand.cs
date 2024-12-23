using Gramium.Framework.Commands;
using Gramium.Framework.Context.Interfaces;
using Gramium.Framework.Extensions;
using Gramium.Framework.States;

namespace Gramium.Examples.BudgetManager.Handlers.Commands;

public class RegisterCommand : CommandBase
{
    public override string Command => "/register";

    public override async Task HandleAsync(IMessageContext context, CancellationToken ct = default)
    {
        await context.SendMessageAsync("Добро пожаловать! Пожалуйста, введите ваше имя:");

        await context.SetStateAsync<RegistrationState, RegistrationStateHandler>(
            context.Message.From!.Id,
            new RegistrationState { Step = "waiting_name" },
            TimeSpan.FromMinutes(5));
    }
}

public class RegistrationStateHandler : BaseStateHandler<RegistrationState>
{
    public override string StateKey => "registration";

    public override async Task<bool> HandleMessageAsync(IMessageContext context, RegistrationState state)
    {
        switch (state.Step)
        {
            case "waiting_name":
                state.Name = context.Message.Text;
                state.Step = "waiting_age";

                await context.SendMessageAsync("Отлично! Теперь введите ваш возраст:");
                await context.SetStateAsync<RegistrationState, RegistrationStateHandler>(
                    context.Message.From!.Id,
                    state,
                    TimeSpan.FromMinutes(5));
                return true;

            case "waiting_age":
                if (!int.TryParse(context.Message.Text, out var age))
                {
                    await context.SendMessageAsync("Пожалуйста, введите корректный возраст:");
                    return false;
                }

                state.Age = age;
                state.Step = "waiting_confirmation";

                var keyboard = context.CreateKeyboard()
                    .WithButtons(("Да", "confirm_registration"), ("Нет", "cancel_registration"))
                    .Build();

                await context.SendMessageAsync(
                    $"Проверьте данные:\nИмя: {state.Name}\nВозраст: {state.Age}\n\nВсё верно?",
                    replyMarkup: keyboard);

                state.Step = "waiting_confirmation";
                await context.SetStateAsync<RegistrationState, RegistrationStateHandler>(
                    context.Message.From!.Id,
                    state,
                    TimeSpan.FromMinutes(5));
                return true;
        }

        return false;
    }

    public override async Task<bool> HandleCallbackQueryAsync(ICallbackQueryContext context, RegistrationState state)
    {
        switch (state.Step)
        {
            case "waiting_confirmation":
            {
                switch (context.CallbackQuery.Data)
                {
                    case "confirm_registration":
                        await context.EditTextMessageAsync(
                            $"Регистрация завершена!\nИмя: {state.Name}\nВозраст: {state.Age}");
                        await context.RemoveStateAsync<RegistrationStateHandler>(context.CallbackQuery.From.Id);
                        return true;

                    case "cancel_registration":
                        await context.EditTextMessageAsync("Регистрация отменена.");
                        await context.RemoveStateAsync<RegistrationStateHandler>(context.CallbackQuery.From.Id);
                        return true;
                }

                return false;
            }
        }

        return false;
    }
}

public class RegistrationState
{
    public string Step { get; set; } = null!;
    public string? Name { get; set; }
    public int? Age { get; set; }
}