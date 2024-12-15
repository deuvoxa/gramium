namespace Gramium.Framework.Context.Interfaces;

public interface ICallbackQueryPayloadContext<out TPayload> : ICallbackQueryContext
    where TPayload : class
{
    TPayload Payload { get; }
}