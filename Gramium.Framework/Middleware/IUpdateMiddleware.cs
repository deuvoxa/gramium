namespace Gramium.Framework.Middleware;

public interface IUpdateMiddleware
{
    Task HandleAsync(UpdateContext context, UpdateDelegate next);
}

public delegate Task UpdateDelegate(UpdateContext context); 