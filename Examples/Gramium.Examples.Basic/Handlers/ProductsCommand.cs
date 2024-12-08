using Gramium.Framework.Commands;
using Gramium.Framework.Context;
using Gramium.Framework.Context.Interfaces;
using Gramium.Framework.Extensions;

namespace Gramium.Examples.Basic.Handlers;

public class ProductsCommand : CommandBase
{
    public override string Command => "/products";
    public override string Description => "Забанить пользователя";
    public override async Task HandleAsync(IMessageContext context, CancellationToken ct = default)
    {
        var args = context.Message.Text!.Split(' ').Skip(1).ToArray();
        var validation = Validate(args);

        if (!validation.IsValid)
        {
            await context.SendMessageAsync(validation.ErrorMessage!);
            return;
        }

        var products = GetProducts();

        await context.CreatePagination(products)
            .ItemsPerPage(5)
            .FormatItem(p => $"{p.Name} - ${p.Price}")
            .WithHeader("Наши продукты:")
            .WithFooter("Страница {0} из {1}")
            .NavigationButtons("Следующая", "Предыдущая")
            .SendAsync();
    }

    private List<Product> GetProducts()
    {
        return
        [
            new Product { Name = "Product 1", Price = 10 },
            new Product { Name = "Product 2", Price = 20 },
            new Product { Name = "Product 3", Price = 20 },
            new Product { Name = "Product 4", Price = 20 },
            new Product { Name = "Product 5", Price = 20 },
            new Product { Name = "Product 6", Price = 20 },
            new Product { Name = "Product 7", Price = 20 },
            new Product { Name = "Product 8", Price = 20 },
            new Product { Name = "Product 9", Price = 20 },
            new Product { Name = "Product 10", Price = 20 },
            new Product { Name = "Product 11", Price = 20 },
            new Product { Name = "Product 12", Price = 20 },
            new Product { Name = "Product 13", Price = 20 },
        ];
    }
}

public class Product
{
    public string Name { get; init; } = string.Empty;
    public decimal Price { get; init; }
}