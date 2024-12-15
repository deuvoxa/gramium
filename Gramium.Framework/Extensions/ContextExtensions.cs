using Gramium.Framework.Context.Interfaces;
using Gramium.Framework.Markup;
using Gramium.Framework.Pagination;

namespace Gramium.Framework.Extensions;

public static class ContextExtensions
{
    public static InlineKeyboardBuilder CreateKeyboard(this IBaseContext context)
    {
        return new InlineKeyboardBuilder();
    }

    public static PaginationBuilder<T> CreatePagination<T>(this IBaseContext context, IEnumerable<T> items)
        where T : class
    {
        return new PaginationBuilder<T>(context, items);
    }
}