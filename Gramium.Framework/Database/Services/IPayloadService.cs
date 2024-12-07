namespace Gramium.Framework.Database.Services;

public interface IPayloadService
{
    Task<string> SavePayloadAsync<TPayload>(string handlerType, TPayload payload) where TPayload : class;
    Task<(string HandlerType, TPayload Payload)> GetPayloadAsync<TPayload>(string id) where TPayload : class;
}