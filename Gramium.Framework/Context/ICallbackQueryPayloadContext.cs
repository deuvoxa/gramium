namespace Gramium.Framework.Context;

public interface ICallbackQueryPayloadContext<out TPayload> : ICallbackQueryContext 
    where TPayload : class
{
    TPayload Payload { get; }
}