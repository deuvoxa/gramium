using Gramium.Framework.Context;
using Gramium.Framework.Context.Interfaces;
using Gramium.Framework.Markup;
using Gramium.Framework.Pagination;

namespace Gramium.Framework.Extensions;

public static class ContextExtensions
{
    public static InlineKeyboardBuilder CreateKeyboard(this IMessageContext context)
    {
        return new InlineKeyboardBuilder();
    }
    
    public static PaginationBuilder<T> CreatePagination<T>(this IMessageContext context, IEnumerable<T> items) where T : class
    {
        return new PaginationBuilder<T>(context, items);
    }
}