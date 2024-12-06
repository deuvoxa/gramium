using Gramium.Framework.Database.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Gramium.Framework.Middleware;

public class UserTrackingMiddleware(IServiceScopeFactory scopeFactory) : IUpdateMiddleware
{
    public async Task HandleAsync(UpdateContext context, UpdateDelegate next)
    {
        using var scope = scopeFactory.CreateScope();
        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
        
        var user = context.Update.Message?.From ?? context.Update.CallbackQuery?.From;
        
        if (user != null)
        {
            await userService.CreateOrUpdateUserAsync(
                user.Id,
                user.FirstName,
                user.Username,
                user.LanguageCode
            );
        }

        await next(context);
    }
} 