using Gramium.Framework.Context.Interfaces;
using System.Text.Json;

namespace Gramium.Framework.States;

public abstract class BaseStateHandler<TState> : IStateHandler<TState> where TState : class, new()
{
    public abstract string StateKey { get; }
    
    public virtual Task<bool> HandleMessageAsync(IMessageContext context, TState state) => Task.FromResult(false);
    public virtual Task<bool> HandleCallbackQueryAsync(ICallbackQueryContext context, TState state) => Task.FromResult(false);
    
    async Task<bool> IStateHandler.HandleMessageAsync(IMessageContext context, string? stateData)
    {
        var state = string.IsNullOrEmpty(stateData) 
            ? new TState() 
            : JsonSerializer.Deserialize<TState>(stateData) ?? new TState();
            
        return await HandleMessageAsync(context, state);
    }

    async Task<bool> IStateHandler.HandleCallbackQueryAsync(ICallbackQueryContext context, string? stateData)
    {
        var state = string.IsNullOrEmpty(stateData) 
            ? new TState() 
            : JsonSerializer.Deserialize<TState>(stateData) ?? new TState();
            
        return await HandleCallbackQueryAsync(context, state);
    }
}