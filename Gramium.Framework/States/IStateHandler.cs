using Gramium.Framework.Context.Interfaces;

namespace Gramium.Framework.States;

public interface IStateHandler
{
    string StateKey { get; }
    Task<bool> HandleMessageAsync(IMessageContext context, string? stateData);
    Task<bool> HandleCallbackQueryAsync(ICallbackQueryContext context, string? stateData);
}

public interface IStateHandler<in TState> : IStateHandler where TState : class, new()
{
    Task<bool> HandleMessageAsync(IMessageContext context, TState state);
    Task<bool> HandleCallbackQueryAsync(ICallbackQueryContext context, TState state);
}