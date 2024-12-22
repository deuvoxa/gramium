using Gramium.Framework.Context.Interfaces;
using Gramium.Framework.Database.Services;
using Gramium.Framework.Markup;
using Gramium.Framework.Pagination;
using Gramium.Framework.States;
using Microsoft.Extensions.DependencyInjection;

namespace Gramium.Framework.Extensions;

public static class ContextExtensions
{
    public static InlineKeyboardBuilder CreateKeyboard(this IBaseContext context)
    {
        return new InlineKeyboardBuilder();
    }

    public static PaginationBuilder<T> CreatePagination<T>(this IBaseContext context, IEnumerable<T> items)
        where T : class
    {
        return new PaginationBuilder<T>(context, items);
    }

    public static async Task SetStateAsync<TState, THandler>(
        this IBaseContext context,
        long userId,
        TState state,
        TimeSpan? expiration = null) 
        where THandler : IStateHandler<TState> 
        where TState : class, new()
    {
        var stateService = context.Services.GetRequiredService<IUserStateService>();
        var handler = context.Services.GetRequiredService<THandler>();
        var expiresAt = expiration.HasValue ? DateTime.UtcNow.Add(expiration.Value) : (DateTime?)null;
        
        var data = System.Text.Json.JsonSerializer.Serialize(state);
        await stateService.SetStateAsync(userId, handler.StateKey, data, expiresAt);
    }

    public static async Task<TState?> GetStateAsync<TState, THandler>(
        this IBaseContext context,
        long userId) 
        where THandler : IStateHandler<TState> 
        where TState : class, new()
    {
        var stateService = context.Services.GetRequiredService<IUserStateService>();
        var handler = context.Services.GetRequiredService<THandler>();
        
        var value = await stateService.GetStateValueAsync(userId, handler.StateKey);
        return string.IsNullOrEmpty(value) ? null : System.Text.Json.JsonSerializer.Deserialize<TState>(value);
    }

    public static async Task RemoveStateAsync<THandler>(
        this IBaseContext context,
        long userId) 
        where THandler : IStateHandler
    {
        var stateService = context.Services.GetRequiredService<IUserStateService>();
        var handler = context.Services.GetRequiredService<THandler>();
        
        await stateService.RemoveStateAsync(userId, handler.StateKey);
    }
}