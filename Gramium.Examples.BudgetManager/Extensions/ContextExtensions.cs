﻿using Gramium.Examples.BudgetManager.Services;
using Gramium.Framework.Context.Interfaces;
using User = Gramium.Examples.BudgetManager.Entities.User;

namespace Gramium.Examples.BudgetManager.Extensions;

public static class ContextExtensions
{
    public static async Task<User> GetUser(this IBaseContext context)
    {
        var userService = context.Services.GetRequiredService<UserService>();
        return context switch
        {
            IMessageContext messageContext => (await userService.GetUserByTelegramIdAsync(messageContext.Message.From!
                .Id))!,
            ICallbackQueryContext callbackQueryContext => (await userService.GetUserByTelegramIdAsync(
                callbackQueryContext.CallbackQuery.From.Id))!,
            _ => default!
        };
    }
}