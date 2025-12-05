using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace namasdev.Core.Types
{
    public static class StringExtensions
    {
        private const string REGEX_NON_ALPHANUMERIC_CHARACTERS_PATTERN = "[^a-zA-Z0-9 ]";

        public static string Capitalize(this string value,
            CultureInfo culture = null)
        {
            return (culture ?? CultureInfo.CurrentCulture).TextInfo.ToTitleCase(value);
        }

        public static string RemoveAccentsAndSpecialCharacters(this string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return String.Empty;
            }

            return new String
                (
                    value.Normalize(NormalizationForm.FormD)
                    .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray()
                )
                .Normalize(NormalizationForm.FormC);
        }

        public static string RemoveNonAlphanumeric(this string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return String.Empty;
            }

            return Regex.Replace(value, REGEX_NON_ALPHANUMERIC_CHARACTERS_PATTERN, String.Empty);
        }

        public static string Truncate(this string value, ushort length,
            string sufix = null)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return String.Empty;
            }

            return length >= value.Length
                ? value
                : value.Substring(0, length) + (sufix ?? String.Empty);
        }

        public static string ValueNotEmptyOrNull(this string value,
            string nullReplacementValue = null)
        {
            return String.IsNullOrWhiteSpace(value)
                ? (nullReplacementValue ?? null)
                : value;
        }

        public static string TrimEnd(this string value, string trimValue)
        {
            return value.EndsWith(trimValue) 
                ? value.Substring(0, value.Length - trimValue.Length) 
                : value;
        }

        public static string ToFirstLetterLowercase(this string value)
        {
            return
                !String.IsNullOrWhiteSpace(value)
                ? value.Substring(0, 1).ToLower() + value.Substring(1)
                : value;
        }
    }
}
