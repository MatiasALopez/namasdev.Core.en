using System;
using System.Collections.Generic;
using System.Linq;

namespace namasdev.Core.Types
{
    public class HtmlHelper
    {
        public static string CreateList<T>(IEnumerable<T> items,
            bool isOrdered = false)
        {
            if (items == null || !items.Any())
            {
                return String.Empty;
            }

            string listTag = isOrdered ? "ol" : "ul";
            return $"<{listTag}>{Formatter.List(items.Select(v => $"<li>{v}</li>"), separator: string.Empty)}</{listTag}>";
        }
    }
}
