using Gramium.Framework.Commands;
using Gramium.Framework.Context;
using Gramium.Framework.Extensions;

namespace Gramium.Examples.Basic.Handlers;

public class Users : CommandBase
{
    public override string Command => "/users";
    public override async Task HandleAsync(IMessageContext context, CancellationToken ct = default)
    {
        var args = context.Message.Text!.Split(' ').Skip(1).ToArray();
        var validation = Validate(args);

        if (!validation.IsValid)
        {
            await context.ReplyAsync(validation.ErrorMessage!);
            return;
        }

        var users = GetUsers();

        await context.CreatePagination(users)
            .ItemsPerPage(3)
            .FormatItem(u => $"{u.Name}, {u.Age} лет. Зарплата ${u.Salary}")
            .WithFooter("\nСтраница {0}/{1}")
            .NavigationButtons("->", "<-")
            .SendAsync();
    }

    private List<User> GetUsers()
    {
        return
        [
            new User { Name = "User a",  Age = 11, Salary = 100 },
            new User { Name = "User b",  Age = 19, Salary = 20000 },
            new User { Name = "User c",  Age = 22, Salary = 50000 },
            new User { Name = "User d",  Age = 0, Salary = 9999999 },
        ];
    }
}

public class User
{
    public string Name { get; init; } = string.Empty;
    public int Age { get; init; }
    public int Salary { get; init; }
}