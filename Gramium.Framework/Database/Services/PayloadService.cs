using System.Text.Json;
using Gramium.Framework.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gramium.Framework.Database.Services;

public class PayloadService(GramiumDbContext context) : IPayloadService
{
    public async Task<string> SavePayloadAsync<TPayload>(string handlerType, TPayload payload)
        where TPayload : class
    {
        var id = Guid.NewGuid().ToString("N");
        var jsonData = JsonSerializer.Serialize(payload);

        var entity = new PayloadEntity
        {
            Id = id,
            HandlerType = handlerType,
            JsonData = jsonData
        };

        context.Payloads.Add(entity);
        await context.SaveChangesAsync();

        return id;
    }

    public async Task<(string HandlerType, TPayload Payload)> GetPayloadAsync<TPayload>(string id)
        where TPayload : class
    {
        var entity = await context.Payloads.FirstOrDefaultAsync(p => p.Id == id)
                     ?? throw new InvalidOperationException($"Payload с ID {id} не найден");

        var payload = JsonSerializer.Deserialize<TPayload>(entity.JsonData)
                      ?? throw new InvalidOperationException($"Не удалось десериализовать payload с ID {id}");

        return (entity.HandlerType, payload);
    }
}