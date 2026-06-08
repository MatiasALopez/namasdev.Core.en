using System;
using namasdev.Core.Types;
using namasdev.Core.Validation;
using Xunit;

namespace namasdev.Core.en.Tests.Validation
{
    // Tests for Validator.Messages. Formatter is used (not Validator.Messages itself) to build the
    // expected value portions, so the assertions verify the message templates' literal text and
    // placeholder ordering. Number/Date/Time messages defer to Formatter with no explicit culture,
    // matching the production code, so the comparisons are culture-independent.
    public class ValidatorMessagesTests
    {
        // Generic / text messages

        [Fact]
        public void Invalid_FormatsName()
        {
            Assert.Equal("Name is not in the valid format.", Validator.Messages.Invalid("Name"));
        }

        [Fact]
        public void TypeInvalid_FormatsNameAndType()
        {
            Assert.Equal("Age is not a valid integer value.", Validator.Messages.TypeInvalid("Age", "integer"));
        }

        [Fact]
        public void MustBeEmpty_FormatsName()
        {
            Assert.Equal("Notes must be empty.", Validator.Messages.MustBeEmpty("Notes"));
        }

        [Fact]
        public void ListNotEmpty_FormatsName()
        {
            Assert.Equal("Items must contain at least one valid element.", Validator.Messages.ListNotEmpty("Items"));
        }

        [Fact]
        public void Required_FormatsName()
        {
            Assert.Equal("Name is required.", Validator.Messages.Required("Name"));
        }

        // Entity messages

        [Fact]
        public void EntityNotFound_NameOnly_OmitsValue()
        {
            Assert.Equal("Customer not found.", Validator.Messages.EntityNotFound("Customer"));
        }

        [Fact]
        public void EntityNotFound_WithSearchValue_IncludesValue()
        {
            Assert.Equal("Customer not found (5).", Validator.Messages.EntityNotFound("Customer", 5));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void EntityNotFound_WithoutSearchValue_OmitsValue(object? searchValue)
        {
            Assert.Equal("Customer not found.", Validator.Messages.EntityNotFound("Customer", searchValue));
        }

        [Fact]
        public void EntityDeleted_FormatsEntityAndValue()
        {
            Assert.Equal("Customer was already deleted (5).", Validator.Messages.EntityDeleted("Customer", 5));
        }

        // Text length messages

        [Fact]
        public void TextLengthMin_FormatsNameAndLength()
        {
            Assert.Equal("Name must be 3 characters long al least.", Validator.Messages.TextLengthMin("Name", 3));
        }

        [Fact]
        public void TextLengthMax_FormatsNameAndLength()
        {
            Assert.Equal("Name must be 5 characters long at most.", Validator.Messages.TextLengthMax("Name", 5));
        }

        [Fact]
        public void TextLengthRange_FormatsNameAndBounds()
        {
            Assert.Equal("Name must be between 2 and 5 characters long.", Validator.Messages.TextLengthRange("Name", 2, 5));
        }

        [Fact]
        public void TextLengthExact_FormatsNameAndLength()
        {
            Assert.Equal("Code must be exactly 5 characters long.", Validator.Messages.TextLengthExact("Code", 5));
        }

        // Simple type-invalid messages

        [Fact]
        public void EmailInvalid_FormatsName()
        {
            Assert.Equal("Email is not a valid email address.", Validator.Messages.EmailInvalid("Email"));
        }

        [Fact]
        public void IPInvalid_FormatsName()
        {
            Assert.Equal("IP is not a valid IP address", Validator.Messages.IPInvalid("IP"));
        }

        [Fact]
        public void IntegerInvalid_FormatsName()
        {
            Assert.Equal("Age must be an integer number.", Validator.Messages.IntegerInvalid("Age"));
        }

        [Fact]
        public void ShortInvalid_FormatsName()
        {
            Assert.Equal("Count must be a short integer number.", Validator.Messages.ShortInvalid("Count"));
        }

        [Fact]
        public void LongInvalid_FormatsName()
        {
            Assert.Equal("Count must be a long integer number.", Validator.Messages.LongInvalid("Count"));
        }

        [Fact]
        public void NumberInvalid_FormatsName()
        {
            Assert.Equal("Amount must be a number.", Validator.Messages.NumberInvalid("Amount"));
        }

        [Fact]
        public void BooleanInvalid_FormatsName()
        {
            Assert.Equal("Active must be a boolean.", Validator.Messages.BooleanInvalid("Active"));
        }

        // Number value messages

        [Fact]
        public void NumberValueMin_Long_FormatsNameAndValue()
        {
            long min = 1000;
            Assert.Equal($"Amount must be a number greater than {Formatter.Number(min)}.",
                Validator.Messages.NumberValueMin("Amount", min));
        }

        [Fact]
        public void NumberValueMin_Decimal_FormatsNameAndValue()
        {
            decimal min = 1000.5m;
            Assert.Equal($"Amount must be a number greater than {Formatter.Number(min, decimalDigits: 2)}.",
                Validator.Messages.NumberValueMin("Amount", min, 2));
        }

        [Fact]
        public void NumberValueMax_Long_FormatsNameAndValue()
        {
            long max = 2500;
            Assert.Equal($"Amount must be a number lower than {Formatter.Number(max)}.",
                Validator.Messages.NumberValueMax("Amount", max));
        }

        [Fact]
        public void NumberValueMax_Decimal_FormatsNameAndValue()
        {
            decimal max = 2500.75m;
            Assert.Equal($"Amount must be a number lower than {Formatter.Number(max, decimalDigits: 2)}.",
                Validator.Messages.NumberValueMax("Amount", max, 2));
        }

        [Fact]
        public void NumberRange_Long_FormatsNameAndBounds()
        {
            long min = 1, max = 1000;
            Assert.Equal($"Amount must be a number between {Formatter.Number(min)} and {Formatter.Number(max)}.",
                Validator.Messages.NumberRange("Amount", min, max));
        }

        [Fact]
        public void NumberRange_Decimal_FormatsNameAndBounds()
        {
            decimal min = 1.5m, max = 1000.25m;
            Assert.Equal($"Amount must be a number between {Formatter.Number(min, decimalDigits: 2)} and {Formatter.Number(max, decimalDigits: 2)}.",
                Validator.Messages.NumberRange("Amount", min, max, 2));
        }

        // Date range messages

        [Fact]
        public void DatesInvalidRange_ExcludeTime_FormatsDates()
        {
            var from = new DateTime(2024, 6, 10);
            var to = new DateTime(2024, 6, 1);
            Assert.Equal($"Invalid date range ({Formatter.Date(from, DateFormat.MDY)} - {Formatter.Date(to, DateFormat.MDY)}).",
                Validator.Messages.DatesInvalidRange(from, to));
        }

        [Fact]
        public void DatesInvalidRange_IncludeTime_FormatsDateTimes()
        {
            var from = new DateTime(2024, 6, 10, 9, 30, 0);
            var to = new DateTime(2024, 6, 1, 8, 0, 0);
            Assert.Equal($"Invalid date range ({Formatter.DateTime(from, DateFormat.MDY)} - {Formatter.DateTime(to, DateFormat.MDY)}).",
                Validator.Messages.DatesInvalidRange(from, to, includeTime: true));
        }

        [Fact]
        public void DatesMonthCountMax_FormatsCount()
        {
            Assert.Equal("Date range cannot exceed 3 months.", Validator.Messages.DatesMonthCountMax(3));
        }

        // Date/time messages

        [Fact]
        public void DateTimeInvalid_FormatsName()
        {
            Assert.Equal("Date must be a valid date/time.", Validator.Messages.DateTimeInvalid("Date"));
        }

        [Fact]
        public void DateTimeMin_ExcludeTime_UsesDateTemplate()
        {
            var min = new DateTime(2024, 1, 1);
            Assert.Equal($"Date must be a date greater than {Formatter.DateTime(min, DateFormat.MDY)}.",
                Validator.Messages.DateTimeMin("Date", min, includeTime: false));
        }

        [Fact]
        public void DateTimeMin_IncludeTime_UsesDateTimeTemplate()
        {
            var min = new DateTime(2024, 1, 1, 9, 30, 0);
            Assert.Equal($"Date must be a date/time greater than {Formatter.DateTime(min, DateFormat.MDY)}.",
                Validator.Messages.DateTimeMin("Date", min, includeTime: true));
        }

        [Fact]
        public void DateTimeMax_ExcludeTime_UsesDateTemplate()
        {
            var max = new DateTime(2024, 12, 31);
            Assert.Equal($"Date must be a date lower than {Formatter.DateTime(max, DateFormat.MDY)}.",
                Validator.Messages.DateTimeMax("Date", max, includeTime: false));
        }

        [Fact]
        public void DateTimeMax_IncludeTime_UsesDateTimeTemplate()
        {
            var max = new DateTime(2024, 12, 31, 23, 59, 0);
            Assert.Equal($"Date must be a date/time lower than {Formatter.DateTime(max, DateFormat.MDY)}.",
                Validator.Messages.DateTimeMax("Date", max, includeTime: true));
        }

        [Fact]
        public void DateTimeRange_ExcludeTime_UsesDateTemplate()
        {
            var from = new DateTime(2024, 1, 1);
            var to = new DateTime(2024, 12, 31);
            Assert.Equal($"Date must be a date between {Formatter.DateTime(from, DateFormat.MDY)} and {Formatter.DateTime(to, DateFormat.MDY)}.",
                Validator.Messages.DateTimeRange("Date", from, to, includeTime: false));
        }

        [Fact]
        public void DateTimeRange_IncludeTime_UsesDateTimeTemplate()
        {
            var from = new DateTime(2024, 1, 1, 8, 0, 0);
            var to = new DateTime(2024, 12, 31, 18, 0, 0);
            Assert.Equal($"Date must be a date/time between {Formatter.DateTime(from, DateFormat.MDY)} and {Formatter.DateTime(to, DateFormat.MDY)}.",
                Validator.Messages.DateTimeRange("Date", from, to, includeTime: true));
        }

        // Time / time-range messages

        [Fact]
        public void TimeInvalid_FormatsName()
        {
            Assert.Equal("Start must be a valid time.", Validator.Messages.TimeInvalid("Start"));
        }

        [Fact]
        public void TimeRangeInvalid_FormatsNameAndBounds()
        {
            var from = new TimeSpan(10, 0, 0);
            var to = new TimeSpan(9, 0, 0);
            Assert.Equal($"Shift is not a valid time range ({Formatter.Time(from)} - {Formatter.Time(to)}).",
                Validator.Messages.TimeRangeInvalid("Shift", from, to));
        }

        [Fact]
        public void TimeRangeMin_FormatsNameAndValue()
        {
            var min = new TimeSpan(1, 0, 0);
            Assert.Equal($"Shift must be a time range of {Formatter.Time(min)} at least.",
                Validator.Messages.TimeRangeMin("Shift", min));
        }

        [Fact]
        public void TimeRangeMax_FormatsNameAndValue()
        {
            var max = new TimeSpan(8, 0, 0);
            Assert.Equal($"Shift must be a time range of {Formatter.Time(max)} at most.",
                Validator.Messages.TimeRangeMax("Shift", max));
        }

        [Fact]
        public void TimeRangeRange_FormatsNameAndBounds()
        {
            var min = new TimeSpan(1, 0, 0);
            var max = new TimeSpan(8, 0, 0);
            Assert.Equal($"Shift must be a time range between {Formatter.Time(min)} and {Formatter.Time(max)}.",
                Validator.Messages.TimeRangeRange("Shift", min, max));
        }

        [Fact]
        public void TimeRangeExact_FormatsNameAndValue()
        {
            var value = new TimeSpan(2, 30, 0);
            Assert.Equal($"Shift must be a time range of {Formatter.Time(value)}.",
                Validator.Messages.TimeRangeExact("Shift", value));
        }

        // File messages

        [Fact]
        public void FileExtensionInvalid_FormatsNameAndExtensions()
        {
            Assert.Equal("File has an invalid file extension. Valid extensions: .pdf, .docx.",
                Validator.Messages.FileExtensionInvalid("File", ".pdf, .docx"));
        }
    }
}
