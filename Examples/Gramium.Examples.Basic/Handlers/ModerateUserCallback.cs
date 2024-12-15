using Gramium.Framework.Callbacks;
using Gramium.Framework.Context.Interfaces;

namespace Gramium.Examples.Basic.Handlers;

public class ModerateUserPayload
{
    public long UserId { get; set; }
    public string Action { get; set; } = null!;
    public int Duration { get; set; }
}

public class ModerateUserCallback : PayloadCallbackBase<ModerateUserPayload>
{
    protected override async Task HandlePayloadAsync(
        ICallbackQueryPayloadContext<ModerateUserPayload> context,
        CancellationToken ct = default)
    {
        var payload = context.Payload; // Строго типизированный payload

        var actionText = payload.Action switch
        {
            "ban" => "забанен",
            "mute" => "замучен",
            _ => throw new ArgumentException($"Неизвестное действие: {payload.Action}")
        };

        await context.EditTextMessageAsync(
            $"Пользователь {payload.UserId} {actionText} на {payload.Duration} минут");
    }
}