using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using namasdev.Core.en.Types;
using namasdev.Core.Validation;

namespace namasdev.Core.Types
{
    public class Formatter
    {
        public const string YES = "Yes";

        public static string Day(DateTime date,
            CultureInfo culture = null)
        {
            return Day(date.DayOfWeek, culture);
        }

        public static string Day(int weekDay,
            CultureInfo culture = null)
        {
            return Day((DayOfWeek)weekDay, culture);
        }

        public static string Day(DayOfWeek weekDay,
            CultureInfo culture = null)
        {
            return (culture ?? CultureInfo.CurrentCulture).DateTimeFormat.GetDayName(weekDay);
        }

        public static string List<T>(IEnumerable<T> values, 
            string separator = ",",
            bool excludeNullOrEmpty = false)
        {
            if (excludeNullOrEmpty)
            {
                values = values.Where(v => !string.IsNullOrWhiteSpace(Convert.ToString(v)));
            }

            return String.Join(separator, values);
        }

        public static string ListHours(IEnumerable<TimeSpan> hours,
            string separator = ",")
        {
            return hours != null
                ? List(hours.Select(h => Time(h)), separator)
                : String.Empty;
        }

        public static string KeyValues<TKey, TValue>(IDictionary<TKey, TValue> dictionary,
            string keyValueSeparator = " : ",
            string itemSeparator = ", ")
        {
            Validator.ValidateRequiredArgumentAndThrow(dictionary, nameof(dictionary));

            return List(dictionary.Select(it => $"{it.Key}{keyValueSeparator}{it.Value}"), separator: itemSeparator);
        }

        public static string Text(IEnumerable<string> lines)
        {
            return lines != null
                ? String.Join(Environment.NewLine, lines)
                : String.Empty;
        }

        public static string TextInLines(string text, int lineCount,
            int? maxLengthInLine = null)
        {
            int textoCantLineas = 1;
            if (maxLengthInLine.HasValue)
            {
                textoCantLineas = (int)Math.Ceiling((decimal)text.Length / maxLengthInLine.Value);
            }
            return $"{text}{String.Join("", Enumerable.Repeat(Environment.NewLine, lineCount > textoCantLineas ? lineCount - textoCantLineas : 0))}";
        }

        public static string FileNameFromUrl(string url)
        {
            return !String.IsNullOrWhiteSpace(url)
                ? WebUtility.UrlDecode(Path.GetFileName(url))
                : String.Empty;
        }

        public static string Number(int number,
            CultureInfo culture = null)
        {
            return culture != null
                ? number.ToString($"N0", culture)
                : number.ToString($"N0");
        }

        public static string Number(int? number,
            CultureInfo culture = null)
        {
            return number.HasValue
                ? Number(number.Value, culture)
                : String.Empty;
        }

        public static string Number(long number,
            CultureInfo culture = null)
        {
            return culture != null
                ? number.ToString($"N0", culture)
                : number.ToString($"N0");
        }

        public static string Number(long? number,
            CultureInfo culture = null)
        {
            return number.HasValue
                ? Number(number.Value, culture)
                : String.Empty;
        }

        public static string Number(decimal number, 
            int decimalDigits = 2,
            CultureInfo culture = null)
        {
            return culture != null
                ? number.ToString($"N{decimalDigits}", culture)
                : number.ToString($"N{decimalDigits}");
        }

        public static string Number(decimal? number,
            int decimalDigits = 2,
            CultureInfo culture = null)
        {
            return number.HasValue 
                ? Number(number.Value, decimalDigits) 
                : String.Empty;
        }

        public static string Number(double number,
            int decimalDigits = 2,
            CultureInfo culture = null)
        {
            return culture != null
                ? number.ToString($"N{decimalDigits}", culture)
                : number.ToString($"N{decimalDigits}");
        }

        public static string Number(double? number,
            int decimalDigits = 2,
            CultureInfo culture = null)
        {
            return number.HasValue
                ? Number(number.Value, decimalDigits)
                : String.Empty;
        }

        public static string EmailList(IEnumerable<string> emails)
        {
            return List(emails);
        }

        public static string Amount(decimal? value,
            int decimalDigits = 2,
            CultureInfo culture = null)
        {
            return value.HasValue
                ? Amount(value.Value, decimalDigits: decimalDigits, culture: culture)
                : String.Empty;
        }

        public static string Amount(decimal value,
            int decimalDigits = 0,
            CultureInfo culture = null)
        {
            return culture != null
                ? value.ToString($"C{decimalDigits}", culture)
                : value.ToString($"C{decimalDigits}");
        }

        public static string Percentage(double value,
            int decimalDigits = 0,
            CultureInfo culture = null)
        {
            return culture != null
                ? value.ToString($"P{decimalDigits}", culture)
                : value.ToString($"P{decimalDigits}");
        }

        public static string Percentage(double? value,
            int decimalDigits = 0,
            CultureInfo culture = null)
        {
            return value.HasValue
                ? Percentage(value.Value, decimalDigits, culture)
                : "";
        }

        public static string Percentage(decimal value,
           int decimalDigits = 0,
           CultureInfo culture = null)
        {
            return culture != null
                ? value.ToString($"P{decimalDigits}", culture)
                : value.ToString($"P{decimalDigits}");
        }

        public static string Percentage(decimal? value,
            int decimalDigits = 0,
            CultureInfo culture = null)
        {
            return value.HasValue
                ? Percentage(value.Value, decimalDigits, culture)
                : "";
        }

        public static string Html(string value)
        {
            return !String.IsNullOrWhiteSpace(value)
                ? value.Replace(Environment.NewLine, "<br/>")
                : null;
        }

        public static string YesNo(bool value)
        {
            return value ? YES : "No";
        }

        public static string DateText(DateTime date,
            bool includeDayName = true)
        {
            return date.ToString($@"{(includeDayName ? "dddd, " : String.Empty)}MMMM dd, yyyy");
        }

        public static string Date(DateTime? date, DateFormat format,
             string emptyText = "",
             string separator = "/")
        {
            return date.HasValue
                ? Date(date.Value, format, separator)
                : emptyText;
        }

        public static string Date(DateTime date, DateFormat format,
            string separator = "/")
        {
            return date.ToString(GetDateFormatString(format, separator));
        }

        private static string GetDateFormatString(DateFormat format, string separator)
        {
            switch (format)
            {
                case DateFormat.DMY:
                    return $"dd{separator}MM{separator}yyyy";
                case DateFormat.MDY:
                    return $"MM{separator}dd{separator}yyyy";
                case DateFormat.YMD:
                    return $"yyyy{separator}MM{separator}dd";
                default:
                    throw new ArgumentException($"Invalid date format ({format}).", nameof(format));
            }
        }

        public static string DateTime(DateTime? dateTime, DateFormat dateFormat,
            string datePartsSeparator = "/", string hourPartsSeparator = ":", string dateTimeSeparator = " ",
            bool includeSeconds = false,
            string emptyText = "")
        {
            return dateTime.HasValue
                ? DateTime(dateTime.Value, dateFormat, datePartsSeparator, hourPartsSeparator, dateTimeSeparator, includeSeconds)
                : emptyText;
        }

        public static string DateTime(DateTime dateTime, DateFormat dateFormat,
            string datePartsSeparator = "/", string hourPartsSeparator = ":", string dateTimeSeparator = " ",
            bool includeSeconds = false)
        {
            return dateTime.ToString($"{GetDateFormatString(dateFormat, datePartsSeparator)}HH{hourPartsSeparator}mm{(includeSeconds ? $"{hourPartsSeparator}ss" : String.Empty)}");
        }

        public static string Time(DateTime? dateTime,
            string emptyText = "")
        {
            return dateTime.HasValue
                ? Time(dateTime.Value)
                : emptyText;
        }

        public static string Time(DateTime dateTime)
        {
            return dateTime.ToString("HH:mm");
        }

        public static string Time(TimeSpan? hour,
            string emptyText = "")
        {
            return hour.HasValue
                ? Time(hour.Value)
                : emptyText;
        }

        public static string Time(TimeSpan hour)
        {
            return hour.ToString("hh\\:mm");
        }

        public static string DateTimeStandard(DateTime dateTime)
        {
            return dateTime.ToString("s");
        }

        public static string TimeCount(TimeSpan? time,
            string emptyText = "")
        {
            return time.HasValue
                ? TimeCount(time)
                : emptyText;
        }

        public static string TimeCount(TimeSpan time)
        {
            var parts = new List<string>();
            if (time.Hours > 0)
            {
                parts.Add($"{time:%h}h");
            }
            if (time.Minutes > 0)
            {
                parts.Add($"{time:mm}m");
            }
            if (time.Seconds > 0)
            {
                parts.Add($"{time:ss}s");
            }
            return List(parts, " ");
        }
    }
}
