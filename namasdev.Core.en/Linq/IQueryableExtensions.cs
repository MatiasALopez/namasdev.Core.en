using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using namasdev.Core.Types;

namespace namasdev.Core.Linq
{
    public static class IQueryableExtensions
    {
        private static readonly ConcurrentDictionary<string, LambdaExpression> _lambdaCache = new ConcurrentDictionary<string, LambdaExpression>();
        private static readonly ConcurrentDictionary<string, MethodInfo> _methodCache = new ConcurrentDictionary<string, MethodInfo>();

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

        public static IOrderedQueryable<T> OrderBy<T>(
            this IQueryable<T> source, string ordering)
        {
            return ApplyOrdering(source, ordering, isFirst: true);
        }

        public static IOrderedQueryable<T> ThenBy<T>(
            this IOrderedQueryable<T> source, string ordering)
        {
            return ApplyOrdering(source, ordering, isFirst: false);
        }

        private static IOrderedQueryable<T> ApplyOrdering<T>(
            IQueryable<T> source, string ordering, bool isFirst)
        {
            if (string.IsNullOrWhiteSpace(ordering))
                throw new ArgumentNullException(nameof(ordering));

            object current = source;
            IOrderedQueryable<T> result = null;

            foreach (var segment in ordering.Split(','))
            {
                var parts = segment.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 0) continue;

                var propertyPath = parts[0];
                var direction = parts.Length > 1 ? parts[1].ToUpperInvariant() : "ASC";

                if (direction != "ASC" && direction != "DESC" &&
                    direction != "ASCENDING" && direction != "DESCENDING")
                    throw new ArgumentException(
                        $"Invalid sort direction '{parts[1]}'. Use ASC, DESC, ASCENDING, or DESCENDING.");

                var descending = direction == "DESC" || direction == "DESCENDING";

                var lambda = GetOrCreateLambda<T>(propertyPath);
                var propType = GetPropertyPathType(typeof(T), propertyPath);

                string methodName =
                    isFirst
                    ? (descending ? "OrderByDescending" : "OrderBy")
                    : (descending ? "ThenByDescending" : "ThenBy");

                result = (IOrderedQueryable<T>)InvokeQueryableMethod(methodName, current, lambda, typeof(T), propType);
                current = result;

                isFirst = false;
            }

            return result;
        }

        private static LambdaExpression GetOrCreateLambda<T>(string propertyPath)
        {
            var key = $"{typeof(T).FullName}.{propertyPath}";
            return _lambdaCache.GetOrAdd(key, _ => BuildLambda<T>(propertyPath));
        }

        private static LambdaExpression BuildLambda<T>(string propertyPath)
        {
            var segments = propertyPath.Split('.');
            var param = Expression.Parameter(typeof(T), "x");

            Expression body = param;
            for (int i = 0; i < segments.Length; i++)
            {
                var prop = body.Type.GetProperty(
                    segments[i],
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)
                    ?? throw new ArgumentException(
                        $"No public property '{segments[i]}' found on type '{body.Type.Name}'. " +
                        $"Full path: '{propertyPath}'.");

                var propAccess = Expression.Property(body, prop);
                var isFinalSegment = i == segments.Length - 1;

                // Null-safe navigation for intermediate reference-type properties.
                // Prevents NullReferenceException on in-memory collections.
                // Also handles Nullable<T> (e.g. int?, DateTime?) at intermediate positions.
                // No-op for EF Core — database handles nulls natively.
                if (!isFinalSegment)
                {
                    var isNullable = !prop.PropertyType.IsValueType ||
                                     Nullable.GetUnderlyingType(prop.PropertyType) != null;

                    if (isNullable)
                    {
                        body = Expression.Condition(
                            Expression.Equal(propAccess, Expression.Constant(null, prop.PropertyType)),
                            Expression.Constant(null, prop.PropertyType),
                            propAccess
                        );
                        continue;
                    }
                }

                body = propAccess;
            }

            return Expression.Lambda(body, param);
        }

        private static Type GetPropertyPathType(Type type, string propertyPath)
        {
            var current = type;
            foreach (var segment in propertyPath.Split('.'))
            {
                var prop = current.GetProperty(
                    segment,
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)
                    ?? throw new ArgumentException(
                        $"No public property '{segment}' found on type '{current.Name}'. " +
                        $"Full path: '{propertyPath}'.");
                current = prop.PropertyType;
            }
            return current;
        }

        private static object InvokeQueryableMethod(
            string methodName,
            object source,
            LambdaExpression lambda,
            Type entityType,
            Type propType)
        {
            var methodKey = $"{methodName}_{entityType.FullName}_{propType.FullName}";

            var method = _methodCache.GetOrAdd(methodKey, _ =>
                typeof(Queryable)
                    .GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                    .MakeGenericMethod(entityType, propType));

            return method.Invoke(null, new object[] { source, lambda });
        }
    }
}
