using System.Collections.Generic;
using System.Linq;

namespace namasdev.Core.Linq
{
    public static class IEnumerableExtensions
    {
        public static bool IsNotNullAndNotEmpty<T>(this IEnumerable<T> list)
        {
            return list != null && list.Any();
        }
    }
}
