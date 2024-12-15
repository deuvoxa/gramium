using Gramium.Framework.Context.Interfaces;

namespace Gramium.Framework.Commands.Interfaces;

public interface ICommandHandler : ICommandMetadata
{
    Task HandleAsync(IMessageContext context, CancellationToken ct = default);
}