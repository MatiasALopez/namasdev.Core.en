using System;
using System.Linq;

namespace namasdev.Core.Types
{
    public class Converter
    {
        public static Guid[] ConvertToGuidList(string list,
            string separator = ",")
        {
            return string.IsNullOrWhiteSpace(list)
                ? null
                : list.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(it => Guid.Parse(it))
                    .ToArray();
        }

        public static short[] ConvertToShortList(string list,
            string separator = ",")
        {
            return string.IsNullOrWhiteSpace(list)
                ? null
                : list.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(it => short.Parse(it))
                    .ToArray();
        }

        public static string[] ConvertToStringList(string list,
            string separator = ",")
        {
            return string.IsNullOrWhiteSpace(list)
                ? null
                : list.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries)
                    .ToArray();
        }
    }
}
