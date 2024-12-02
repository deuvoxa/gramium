namespace Gramium.Framework.Middleware;

public class MiddlewarePipeline
{
    private readonly List<IUpdateMiddleware> _middleware = new();

    public void Use(IUpdateMiddleware middleware)
    {
        _middleware.Add(middleware);
    }

    public async Task ExecuteAsync(UpdateContext context)
    {
        var index = 0;

        UpdateDelegate next = null!;
        next = async (ctx) =>
        {
            if (index < _middleware.Count)
            {
                var current = _middleware[index];
                index++;
                await current.HandleAsync(ctx, next);
            }
        };

        await next(context);
    }
}