using Gramium.Framework.Context;

namespace Gramium.Framework.Interfaces;

public interface ICommandHandler
{
    string Command { get; }
    Task HandleAsync(IMessageContext context, CancellationToken ct = default);
} 