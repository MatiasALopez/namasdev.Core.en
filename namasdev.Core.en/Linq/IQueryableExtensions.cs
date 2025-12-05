using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;

using namasdev.Core.Types;

namespace namasdev.Core.Linq
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, Expression<Func<T, bool>> predicate, bool condition)
        {
            return condition
                ? query.Where(predicate)
                : query;
        }

        public static IQueryable<T> Order<T>(this IQueryable<T> query, string order)
        {
            return
                String.IsNullOrWhiteSpace(order)
                ? query
                : query.OrderBy(order);
        }

        public static IQueryable<T> OrderAndPage<T>(this IQueryable<T> query, OrderAndPagingParameters op,
            string defaultOrder = null)
        {
            defaultOrder = (defaultOrder ?? op?.DefaultOrder)
                .ValueNotEmptyOrNull(nullReplacementValue: "1");

            if (op == null)
            {
                return !String.IsNullOrWhiteSpace(defaultOrder)
                    ? query.OrderBy(defaultOrder)
                    : query;
            }

            string order =
                !String.IsNullOrWhiteSpace(op.Order)
                ? op.Order
                : defaultOrder;

            op.ItemsTotalCount = query.Count();

            return query
                .Order(order)
                .Skip((Math.Max(op.Page, 1) - 1) * op.ItemsPerPage)
                .Take(op.ItemsPerPage);
        }

        public static IQueryable<T> Page<T>(this IQueryable<T> query, int page, int itemsPerPage)
        {
            return query
                .Skip((Math.Max(page, 1) - 1) * itemsPerPage)
                .Take(itemsPerPage);
        }
    }
}
