using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using namasdev.Core.Types;
using Xunit;

namespace namasdev.Core.en.Tests.Types
{
    public class FormatterTests
    {
        private static readonly CultureInfo EnUs = new CultureInfo("en-US");

        // YesNo

        [Fact]
        public void YesNo_True_ReturnsYes()
        {
            Assert.Equal("Yes", Formatter.YesNo(true));
        }

        [Fact]
        public void YesNo_False_ReturnsNo()
        {
            Assert.Equal("No", Formatter.YesNo(false));
        }

        // Number

        [Fact]
        public void Number_Int_FormatsWithThousandsSeparator()
        {
            Assert.Equal("1,000", Formatter.Number(1000, EnUs));
        }

        [Fact]
        public void Number_NullInt_ReturnsEmpty()
        {
            Assert.Equal(string.Empty, Formatter.Number((int?)null));
        }

        [Fact]
        public void Number_Decimal_FormatsWithTwoDecimals()
        {
            Assert.Equal("1,234.56", Formatter.Number(1234.56m, decimalDigits: 2, culture: EnUs));
        }

        [Fact]
        public void Number_NullDecimal_ReturnsEmpty()
        {
            Assert.Equal(string.Empty, Formatter.Number((decimal?)null));
        }

        // Date

        [Fact]
        public void Date_DMY_FormatsCorrectly()
        {
            var date = new DateTime(2024, 3, 15);
            Assert.Equal("15/03/2024", Formatter.Date(date, DateFormat.DMY));
        }

        [Fact]
        public void Date_MDY_FormatsCorrectly()
        {
            var date = new DateTime(2024, 3, 15);
            Assert.Equal("03/15/2024", Formatter.Date(date, DateFormat.MDY));
        }

        [Fact]
        public void Date_YMD_FormatsCorrectly()
        {
            var date = new DateTime(2024, 3, 15);
            Assert.Equal("2024/03/15", Formatter.Date(date, DateFormat.YMD));
        }

        [Fact]
        public void Date_Null_ReturnsEmptyText()
        {
            Assert.Equal("N/A", Formatter.Date((DateTime?)null, DateFormat.MDY, "N/A"));
        }

        [Fact]
        public void Date_CustomSeparator_UsesIt()
        {
            var date = new DateTime(2024, 3, 15);
            Assert.Equal("2024-03-15", Formatter.Date(date, DateFormat.YMD, "-"));
        }

        // Time

        [Fact]
        public void Time_DateTime_FormatsHHmm()
        {
            var dt = new DateTime(2024, 1, 1, 9, 5, 0);
            Assert.Equal("09:05", Formatter.Time(dt));
        }

        [Fact]
        public void Time_NullDateTime_ReturnsEmptyText()
        {
            Assert.Equal("--", Formatter.Time((DateTime?)null, "--"));
        }

        [Fact]
        public void Time_TimeSpan_FormatsHHmm()
        {
            Assert.Equal("08:30", Formatter.Time(new TimeSpan(8, 30, 0)));
        }

        [Fact]
        public void Time_NullTimeSpan_ReturnsEmpty()
        {
            Assert.Equal(string.Empty, Formatter.Time((TimeSpan?)null));
        }

        // DateText — Formatter.DateText uses DateTime.ToString without CultureInfo,
        // so tests must set thread culture explicitly.

        [Fact]
        public void DateText_IncludeDayName_FormatsCorrectly()
        {
            var prev = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = EnUs;
            try
            {
                var date = new DateTime(2024, 1, 15); // Monday
                var result = Formatter.DateText(date, includeDayName: true);
                Assert.Contains("January 15, 2024", result);
            }
            finally { Thread.CurrentThread.CurrentCulture = prev; }
        }

        [Fact]
        public void DateText_ExcludeDayName_OmitsDayName()
        {
            var prev = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = EnUs;
            try
            {
                var date = new DateTime(2024, 1, 15);
                var result = Formatter.DateText(date, includeDayName: false);
                Assert.Equal("January 15, 2024", result);
            }
            finally { Thread.CurrentThread.CurrentCulture = prev; }
        }

        // List

        [Fact]
        public void List_WithSeparator_JoinsValues()
        {
            Assert.Equal("a|b|c", Formatter.List(new[] { "a", "b", "c" }, "|"));
        }

        [Fact]
        public void List_ExcludeNullOrEmpty_SkipsEmpties()
        {
            Assert.Equal("a,c", Formatter.List(new[] { "a", "", "c" }, ",", excludeNullOrEmpty: true));
        }

        // Html

        [Fact]
        public void Html_ReplacesNewlineWithBr()
        {
            var input = $"line1{Environment.NewLine}line2";
            Assert.Equal("line1<br/>line2", Formatter.Html(input));
        }

        [Fact]
        public void Html_NullOrWhitespace_ReturnsNull()
        {
            Assert.Null(Formatter.Html("  "));
        }

        // FileNameFromUrl

        [Fact]
        public void FileNameFromUrl_ExtractsFileName()
        {
            Assert.Equal("file.pdf", Formatter.FileNameFromUrl("http://example.com/docs/file.pdf"));
        }

        [Fact]
        public void FileNameFromUrl_Null_ReturnsEmpty()
        {
            Assert.Equal(string.Empty, Formatter.FileNameFromUrl(null));
        }

        // KeyValues

        [Fact]
        public void KeyValues_FormatsDictionary()
        {
            var dict = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };
            var result = Formatter.KeyValues(dict);
            Assert.Contains("a : 1", result);
            Assert.Contains("b : 2", result);
        }

        // Text

        [Fact]
        public void Text_JoinsLinesWithNewLine()
        {
            var lines = new[] { "line1", "line2" };
            Assert.Equal($"line1{Environment.NewLine}line2", Formatter.Text(lines));
        }

        [Fact]
        public void Text_Null_ReturnsEmpty()
        {
            Assert.Equal(string.Empty, Formatter.Text(null));
        }

        // Day

        [Fact]
        public void Day_DayOfWeek_ReturnsName()
        {
            Assert.Equal("Monday", Formatter.Day(DayOfWeek.Monday, EnUs));
        }
    }
}
