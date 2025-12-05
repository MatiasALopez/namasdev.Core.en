using System;
using System.Linq;

namespace namasdev.Core
{
    public class MathHelper
    {
        public static decimal? Max(params decimal?[] values)
        {
            if (values.Length == 0)
            {
                return null;
            }

            return values.Max();
        }

        public static double? Max(params double?[] values)
        {
            if (values.Length == 0)
            {
                return null;
            }

            return values.Max();
        }

        public static decimal? Min(params decimal?[] values)
        {
            if (values.Length == 0)
            {
                return null;
            }

            return values.Min();
        }

        public static double? Min(params double?[] values)
        {
            if (values.Length == 0)
            {
                return null;
            }

            return values.Min();
        }

        public static double Sum(double[] values,
            int? roundingDigits = null)
        {
            return Round(values.Sum(), roundingDigits);
        }

        public static double? Sum(double?[] values,
            int? roundingDigits = null)
        {
            var sum = values.Sum();
            return sum.HasValue
                ? Round(sum.Value, roundingDigits)
                : (double?)null;
        }

        public static decimal? Sum(decimal?[] values,
            int? roundingDigits = null)
        {
            var sum = values.Sum();
            return sum.HasValue
                ? Round(sum.Value, roundingDigits)
                : (decimal?)null;
        }

        public static double Average(double[] values,
            int? roundingDigits = null)
        {
            return Round(values.Average(), roundingDigits);
        }

        public static double? Average(double?[] values,
            int? roundingDigits = null)
        {
            var avg = values.Average();
            return avg.HasValue
                ? Round(avg.Value, roundingDigits)
                : (double?)null;
        }

        public static decimal? Average(decimal?[] values, 
            int? roundingDigits = null)
        {
            var avg = values.Average();
            return avg.HasValue
                ? Round(avg.Value, roundingDigits)
                : (decimal?)null;
        }

        public static decimal? Average(decimal? sum, int? totalCount,
            int? roundingDigits = null)
        {
            return sum.HasValue && totalCount.HasValue
                ? Average(sum.Value, totalCount.Value, roundingDigits)
                : (decimal?)null;
        }

        public static decimal Average(decimal sum, int totalCount,
            int? roundingDigits = null)
        {
            return totalCount > 0
                ? Round(sum / totalCount, roundingDigits) 
                : 0;
        }

        public static double? Average(double? sum, int? totalCount,
            int? roundingDigits = null)
        {
            return sum.HasValue && totalCount.HasValue
                ? Average(sum.Value, totalCount.Value, roundingDigits)
                : (double?)null;
        }

        public static double Average(double sum, int totalCount,
            int? roundingDigits = null)
        {
            return totalCount > 0
                ? Round(sum / totalCount, roundingDigits)
                : 0;
        }

        public static decimal? Percentage(int? count, int totalCount,
            int? roundingDigits = null)
        {
            return count.HasValue
                ? Percentage(count.Value, totalCount, roundingDigits)
                : (decimal?)null;
        }

        public static decimal Percentage(int count, int totalCount,
            int? roundingDigits = null)
        {
            return totalCount > 0 
                ? Round((decimal)count / totalCount, roundingDigits)
                : 0;
        }

        public static decimal? PercentageIf(bool condition, int? count, int totalCount,
            int? roundingDigits = null)
        {
            return condition && count.HasValue
                ? Percentage(count.Value, totalCount, roundingDigits)
                : (decimal?)null;
        }

        public static int CountNotNull(params decimal?[] values)
        {
            return values.Count(v => v.HasValue);
        }

        public static int CountNotNull(params double?[] values)
        {
            return values.Count(v => v.HasValue);
        }

        public static int CountTrue(params bool[] values)
        {
            return values.Count(v => v);
        }

        private static decimal Round(decimal value, int? roundingDigits)
        {
            return roundingDigits.HasValue
                ? Math.Round(value, roundingDigits.Value)
                : value;
        }

        private static double Round(double value, int? roundingDigits)
        {
            return roundingDigits.HasValue
                ? Math.Round(value, roundingDigits.Value)
                : value;
        }
    }
}
