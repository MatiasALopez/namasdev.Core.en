using System;
using System.Collections.Generic;
using namasdev.Core.Exceptions;
using namasdev.Core.Validation;
using Xunit;

namespace namasdev.Core.en.Tests.Validation
{
    public class ValidatorTests
    {
        // ValidateString

        [Fact]
        public void ValidateString_RequiredAndEmpty_Fails()
        {
            var ok = Validator.ValidateString("  ", "Name", required: true, out string error);
            Assert.False(ok);
            Assert.Equal(Validator.Messages.Required("Name"), error);
        }

        [Fact]
        public void ValidateString_NotRequiredAndEmpty_Passes()
        {
            var ok = Validator.ValidateString(null, "Name", required: false, out string error);
            Assert.True(ok);
            Assert.Null(error);
        }

        [Fact]
        public void ValidateString_ExactLengthMismatch_Fails()
        {
            var ok = Validator.ValidateString("abc", "Code", required: true, out string error, exactLength: 5);
            Assert.False(ok);
            Assert.Equal(Validator.Messages.TextLengthExact("Code", 5), error);
        }

        [Fact]
        public void ValidateString_ExactLengthMatch_Passes()
        {
            var ok = Validator.ValidateString("abcde", "Code", required: true, out string error, exactLength: 5);
            Assert.True(ok);
            Assert.Null(error);
        }

        [Fact]
        public void ValidateString_BelowMinLength_Fails()
        {
            var ok = Validator.ValidateString("ab", "Name", required: true, out string error, minLength: 3);
            Assert.False(ok);
            Assert.Equal(Validator.Messages.TextLengthMin("Name", 3), error);
        }

        [Fact]
        public void ValidateString_AboveMaxLength_Fails()
        {
            var ok = Validator.ValidateString("abcdef", "Name", required: true, out string error, maxLength: 5);
            Assert.False(ok);
            Assert.Equal(Validator.Messages.TextLengthMax("Name", 5), error);
        }

        [Fact]
        public void ValidateString_OutsideRange_Fails()
        {
            var ok = Validator.ValidateString("abcdef", "Name", required: true, out string error, minLength: 2, maxLength: 5);
            Assert.False(ok);
            Assert.Equal(Validator.Messages.TextLengthRange("Name", 2, 5), error);
        }

        [Fact]
        public void ValidateString_WithinRange_Passes()
        {
            var ok = Validator.ValidateString("abc", "Name", required: true, out string error, minLength: 2, maxLength: 5);
            Assert.True(ok);
            Assert.Null(error);
        }

        [Fact]
        public void ValidateString_RegExMismatch_Fails()
        {
            var ok = Validator.ValidateString("abc", "Code", required: true, out string error, regEx: @"^\d+$");
            Assert.False(ok);
            Assert.Equal(Validator.Messages.Invalid("Code"), error);
        }

        [Fact]
        public void ValidateString_RegExMatch_Passes()
        {
            var ok = Validator.ValidateString("123", "Code", required: true, out string error, regEx: @"^\d+$");
            Assert.True(ok);
            Assert.Null(error);
        }

        // ValidateStringAndAddToErrorList

        [Fact]
        public void ValidateStringAndAddToErrorList_Invalid_AddsError()
        {
            var errors = new List<string>();
            var ok = Validator.ValidateStringAndAddToErrorList(null, "Name", required: true, errors);
            Assert.False(ok);
            Assert.Single(errors);
            Assert.Equal(Validator.Messages.Required("Name"), errors[0]);
        }

        [Fact]
        public void ValidateStringAndAddToErrorList_Valid_DoesNotAddError()
        {
            var errors = new List<string>();
            var ok = Validator.ValidateStringAndAddToErrorList("abc", "Name", required: true, errors);
            Assert.True(ok);
            Assert.Empty(errors);
        }

        // ValidateNumber (decimal)

        [Fact]
        public void ValidateNumber_RequiredAndNull_Fails()
        {
            var ok = Validator.ValidateNumber((decimal?)null, "Amount", required: true, out string error);
            Assert.False(ok);
            Assert.Equal(Validator.Messages.Required("Amount"), error);
        }

        [Fact]
        public void ValidateNumber_NotRequiredAndNull_Passes()
        {
            var ok = Validator.ValidateNumber((decimal?)null, "Amount", required: false, out string error);
            Assert.True(ok);
            Assert.Null(error);
        }

        [Fact]
        public void ValidateNumber_BelowMin_Fails()
        {
            var ok = Validator.ValidateNumber(5m, "Amount", required: true, out string error, minValue: 10m);
            Assert.False(ok);
            Assert.Equal(Validator.Messages.NumberValueMin("Amount", 10m, 2), error);
        }

        [Fact]
        public void ValidateNumber_AboveMax_Fails()
        {
            var ok = Validator.ValidateNumber(15m, "Amount", required: true, out string error, maxValue: 10m);
            Assert.False(ok);
            Assert.Equal(Validator.Messages.NumberValueMax("Amount", 10m, 2), error);
        }

        [Fact]
        public void ValidateNumber_OutsideRange_Fails()
        {
            var ok = Validator.ValidateNumber(20m, "Amount", required: true, out string error, minValue: 1m, maxValue: 10m);
            Assert.False(ok);
            Assert.Equal(Validator.Messages.NumberRange("Amount", 1m, 10m, 2), error);
        }

        [Fact]
        public void ValidateNumber_WithinRange_Passes()
        {
            var ok = Validator.ValidateNumber(5m, "Amount", required: true, out string error, minValue: 1m, maxValue: 10m);
            Assert.True(ok);
            Assert.Null(error);
        }

        // ValidateNumber (int overload)

        [Fact]
        public void ValidateNumber_Int_BelowMin_Fails()
        {
            var ok = Validator.ValidateNumber((int?)2, "Qty", required: true, out string error, minValue: 5);
            Assert.False(ok);
            Assert.Equal(Validator.Messages.NumberValueMin("Qty", 5L), error);
        }

        [Fact]
        public void ValidateNumber_Int_Valid_Passes()
        {
            var ok = Validator.ValidateNumber((int?)7, "Qty", required: true, out string error, minValue: 5, maxValue: 10);
            Assert.True(ok);
            Assert.Null(error);
        }

        // ValidateEmail

        [Theory]
        [InlineData("user@example.com")]
        [InlineData("first.last@sub.example.co")]
        public void ValidateEmail_Valid_Passes(string email)
        {
            var ok = Validator.ValidateEmail(email, "Email", required: true, out string error);
            Assert.True(ok);
            Assert.Null(error);
        }

        [Theory]
        [InlineData("not-an-email")]
        [InlineData("missing@")]
        [InlineData("@nodomain")]
        public void ValidateEmail_Invalid_Fails(string email)
        {
            var ok = Validator.ValidateEmail(email, "Email", required: true, out string error);
            Assert.False(ok);
            Assert.Equal(Validator.Messages.EmailInvalid("Email"), error);
        }

        [Fact]
        public void ValidateEmail_RequiredAndEmpty_FailsAsRequired()
        {
            var ok = Validator.ValidateEmail("", "Email", required: true, out string error);
            Assert.False(ok);
            Assert.Equal(Validator.Messages.Required("Email"), error);
        }

        [Fact]
        public void ValidateEmail_NotRequiredAndEmpty_Passes()
        {
            var ok = Validator.ValidateEmail(null, "Email", required: false, out string error);
            Assert.True(ok);
            Assert.Null(error);
        }

        // ValidateIP

        [Theory]
        [InlineData("192.168.0.1")]
        [InlineData("10.0.0.255")]
        public void ValidateIP_Valid_Passes(string ip)
        {
            var ok = Validator.ValidateIP(ip, "IP", out string error);
            Assert.True(ok);
            Assert.Null(error);
        }

        [Theory]
        [InlineData("999.999.999.999")]
        [InlineData("not-an-ip")]
        [InlineData("192.168.0")]
        public void ValidateIP_Invalid_Fails(string ip)
        {
            var ok = Validator.ValidateIP(ip, "IP", out string error);
            Assert.False(ok);
            Assert.Equal(Validator.Messages.IPInvalid("IP"), error);
        }

        // ValidateDate

        [Fact]
        public void ValidateDate_RequiredAndNull_Fails()
        {
            var ok = Validator.ValidateDate(null, "Date", required: true, out string error);
            Assert.False(ok);
            Assert.Equal(Validator.Messages.Required("Date"), error);
        }

        [Fact]
        public void ValidateDate_NotRequiredAndNull_Passes()
        {
            var ok = Validator.ValidateDate(null, "Date", required: false, out string error);
            Assert.True(ok);
            Assert.Null(error);
        }

        [Fact]
        public void ValidateDate_BeforeMin_Fails()
        {
            var min = new DateTime(2024, 1, 1);
            var ok = Validator.ValidateDate(new DateTime(2023, 12, 31), "Date", required: true, out string error, minValue: min);
            Assert.False(ok);
            Assert.Equal(Validator.Messages.DateTimeMin("Date", min), error);
        }

        [Fact]
        public void ValidateDate_AfterMax_Fails()
        {
            var max = new DateTime(2024, 1, 1);
            var ok = Validator.ValidateDate(new DateTime(2024, 1, 2), "Date", required: true, out string error, maxValue: max);
            Assert.False(ok);
            Assert.Equal(Validator.Messages.DateTimeMax("Date", max), error);
        }

        [Fact]
        public void ValidateDate_WithinRange_Passes()
        {
            var min = new DateTime(2024, 1, 1);
            var max = new DateTime(2024, 12, 31);
            var ok = Validator.ValidateDate(new DateTime(2024, 6, 15), "Date", required: true, out string error, minValue: min, maxValue: max);
            Assert.True(ok);
            Assert.Null(error);
        }

        [Fact]
        public void ValidateDate_ExcludeTime_IgnoresTimeComponent()
        {
            var max = new DateTime(2024, 6, 15, 0, 0, 0);
            // Same date, later time — with includeTime: false the time is stripped, so it passes.
            var ok = Validator.ValidateDate(new DateTime(2024, 6, 15, 23, 59, 0), "Date", required: true, out string error, maxValue: max, includeTime: false);
            Assert.True(ok);
            Assert.Null(error);
        }

        // ValidateDateRange

        [Fact]
        public void ValidateDateRange_FromAfterTo_Fails()
        {
            var from = new DateTime(2024, 6, 10);
            var to = new DateTime(2024, 6, 1);
            var ok = Validator.ValidateDateRange(from, to, out string error);
            Assert.False(ok);
            Assert.Equal(Validator.Messages.DatesInvalidRange(from, to), error);
        }

        [Fact]
        public void ValidateDateRange_ValidRange_Passes()
        {
            var ok = Validator.ValidateDateRange(new DateTime(2024, 6, 1), new DateTime(2024, 6, 10), out string error);
            Assert.True(ok);
            Assert.Null(error);
        }

        [Fact]
        public void ValidateDateRange_ExceedsMonthCountMax_Fails()
        {
            var from = new DateTime(2024, 1, 1);
            var to = new DateTime(2024, 6, 1);
            var ok = Validator.ValidateDateRange(from, to, out string error, monthCountMax: 3);
            Assert.False(ok);
            Assert.Equal(Validator.Messages.DatesMonthCountMax(3), error);
        }

        [Fact]
        public void ValidateDateRange_WithinMonthCountMax_Passes()
        {
            var from = new DateTime(2024, 1, 1);
            var to = new DateTime(2024, 2, 1);
            var ok = Validator.ValidateDateRange(from, to, out string error, monthCountMax: 3);
            Assert.True(ok);
            Assert.Null(error);
        }

        // ValidateTimeRange

        [Fact]
        public void ValidateTimeRange_FromAfterTo_Fails()
        {
            var from = new TimeSpan(10, 0, 0);
            var to = new TimeSpan(9, 0, 0);
            var ok = Validator.ValidateTimeRange(from, to, "Time", out string error);
            Assert.False(ok);
            Assert.Equal(Validator.Messages.TimeRangeInvalid("Time", from, to), error);
        }

        [Fact]
        public void ValidateTimeRange_ExactValueMismatch_Fails()
        {
            var from = new TimeSpan(9, 0, 0);
            var to = new TimeSpan(10, 0, 0);
            var exact = new TimeSpan(2, 0, 0);
            var ok = Validator.ValidateTimeRange(from, to, "Time", out string error, exactValue: exact);
            Assert.False(ok);
            Assert.Equal(Validator.Messages.TimeRangeExact("Time", exact), error);
        }

        [Fact]
        public void ValidateTimeRange_ExactValueMatch_Passes()
        {
            var from = new TimeSpan(9, 0, 0);
            var to = new TimeSpan(11, 0, 0);
            var ok = Validator.ValidateTimeRange(from, to, "Time", out string error, exactValue: new TimeSpan(2, 0, 0));
            Assert.True(ok);
            Assert.Null(error);
        }

        [Fact]
        public void ValidateTimeRange_BelowMin_Fails()
        {
            var from = new TimeSpan(9, 0, 0);
            var to = new TimeSpan(9, 30, 0);
            var min = new TimeSpan(1, 0, 0);
            var ok = Validator.ValidateTimeRange(from, to, "Time", out string error, minValue: min);
            Assert.False(ok);
            Assert.Equal(Validator.Messages.TimeRangeMin("Time", min), error);
        }

        [Fact]
        public void ValidateTimeRange_AboveMax_Fails()
        {
            var from = new TimeSpan(9, 0, 0);
            var to = new TimeSpan(12, 0, 0);
            var max = new TimeSpan(2, 0, 0);
            var ok = Validator.ValidateTimeRange(from, to, "Time", out string error, maxValue: max);
            Assert.False(ok);
            Assert.Equal(Validator.Messages.TimeRangeMax("Time", max), error);
        }

        [Fact]
        public void ValidateTimeRange_WithinRange_Passes()
        {
            var from = new TimeSpan(9, 0, 0);
            var to = new TimeSpan(10, 30, 0);
            var ok = Validator.ValidateTimeRange(from, to, "Time", out string error,
                minValue: new TimeSpan(1, 0, 0), maxValue: new TimeSpan(2, 0, 0));
            Assert.True(ok);
            Assert.Null(error);
        }

        // IsValidFileExtension / ValidateFileExtension

        [Fact]
        public void IsValidFileExtension_MatchingExtension_ReturnsTrue()
        {
            Assert.True(Validator.IsValidFileExtension("report.pdf", ".pdf,.docx"));
        }

        [Fact]
        public void IsValidFileExtension_NonMatchingExtension_ReturnsFalse()
        {
            Assert.False(Validator.IsValidFileExtension("report.txt", ".pdf,.docx"));
        }

        [Fact]
        public void IsValidFileExtension_IsCaseInsensitive()
        {
            Assert.True(Validator.IsValidFileExtension("REPORT.PDF", ".pdf"));
        }

        [Fact]
        public void IsValidFileExtension_NullExtensionList_Throws()
        {
            Assert.Throws<ArgumentNullException>(
                () => Validator.IsValidFileExtension("report.pdf", (string)null));
        }

        [Fact]
        public void ValidateFileExtension_Invalid_Fails()
        {
            var ok = Validator.ValidateFileExtension("report.txt", ".pdf", out string error, description: "Report");
            Assert.False(ok);
            Assert.Equal(Validator.Messages.FileExtensionInvalid("Report", ".pdf"), error);
        }

        [Fact]
        public void ValidateFileExtension_Valid_Passes()
        {
            var ok = Validator.ValidateFileExtension("report.pdf", ".pdf", out string error);
            Assert.True(ok);
            Assert.Null(error);
        }

        // ValidateFile

        [Fact]
        public void ValidateFile_RequiredAndNull_Fails()
        {
            var ok = Validator.ValidateFile(null, required: true, out string error, description: "Attachment", extensionList: (string)null);
            Assert.False(ok);
            Assert.Equal(Validator.Messages.Required("Attachment"), error);
        }

        [Fact]
        public void ValidateFile_NotRequiredAndEmpty_Passes()
        {
            var file = new Core.IO.File { Name = "empty.pdf", Content = new byte[0] };
            var ok = Validator.ValidateFile(file, required: false, out string error, extensionList: (string)null);
            Assert.True(ok);
            Assert.Null(error);
        }

        [Fact]
        public void ValidateFile_WithContentAndValidExtension_Passes()
        {
            var file = new Core.IO.File { Name = "doc.pdf", Content = new byte[] { 1, 2, 3 } };
            var ok = Validator.ValidateFile(file, required: true, out string error, extensionList: new[] { ".pdf" });
            Assert.True(ok);
            Assert.Null(error);
        }

        [Fact]
        public void ValidateFile_WithContentAndInvalidExtension_Fails()
        {
            var file = new Core.IO.File { Name = "doc.txt", Content = new byte[] { 1, 2, 3 } };
            var ok = Validator.ValidateFile(file, required: true, out string error, extensionList: new[] { ".pdf" });
            Assert.False(ok);
            Assert.Equal(Validator.Messages.FileExtensionInvalid("doc.txt", ".pdf"), error);
        }

        // ValidateRequiredAndThrow / ValidateRequiredArgumentAndThrow

        [Fact]
        public void ValidateRequiredArgumentAndThrow_Null_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => Validator.ValidateRequiredArgumentAndThrow(null, "arg"));
        }

        [Fact]
        public void ValidateRequiredArgumentAndThrow_EmptyString_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => Validator.ValidateRequiredArgumentAndThrow("  ", "arg"));
        }

        [Fact]
        public void ValidateRequiredArgumentAndThrow_NonEmpty_DoesNotThrow()
        {
            var ex = Record.Exception(() => Validator.ValidateRequiredArgumentAndThrow("value", "arg"));
            Assert.Null(ex);
        }

        [Fact]
        public void ValidateRequiredAndThrow_CustomException_IsThrown()
        {
            Assert.Throws<ExceptionFriendlyMessage>(
                () => Validator.ValidateRequiredAndThrow<ExceptionFriendlyMessage>(null, "required"));
        }

        // ValidateRequiredListArgumentAndThrow

        [Fact]
        public void ValidateRequiredListArgumentAndThrow_Null_Throws()
        {
            Assert.Throws<ArgumentNullException>(
                () => Validator.ValidateRequiredListArgumentAndThrow<int>(null, "list"));
        }

        [Fact]
        public void ValidateRequiredListArgumentAndThrow_Empty_Throws()
        {
            Assert.Throws<ArgumentNullException>(
                () => Validator.ValidateRequiredListArgumentAndThrow(new List<int>(), "list"));
        }

        [Fact]
        public void ValidateRequiredListArgumentAndThrow_EmptyButNotValidatingNotEmpty_DoesNotThrow()
        {
            var ex = Record.Exception(
                () => Validator.ValidateRequiredListArgumentAndThrow(new List<int>(), "list", validateNotEmpty: false));
            Assert.Null(ex);
        }

        [Fact]
        public void ValidateRequiredListArgumentAndThrow_NonEmpty_DoesNotThrow()
        {
            var ex = Record.Exception(
                () => Validator.ValidateRequiredListArgumentAndThrow(new List<int> { 1 }, "list"));
            Assert.Null(ex);
        }

        // ValidateEntityNotNullAndThrow

        [Fact]
        public void ValidateEntityNotNullAndThrow_Null_Throws()
        {
            var ex = Assert.Throws<Exception>(
                () => Validator.ValidateEntityNotNullAndThrow(null, 5, "Customer"));
            Assert.Equal(Validator.Messages.EntityNotFound("Customer", 5), ex.Message);
        }

        [Fact]
        public void ValidateEntityNotNullAndThrow_NotNull_DoesNotThrow()
        {
            var ex = Record.Exception(
                () => Validator.ValidateEntityNotNullAndThrow(new object(), 5, "Customer"));
            Assert.Null(ex);
        }

        // ThrowExceptionFriendlyMessageIfAnyErrors

        [Fact]
        public void ThrowExceptionFriendlyMessageIfAnyErrors_WithErrors_Throws()
        {
            var errors = new List<string> { "Error 1", "Error 2" };
            var ex = Assert.Throws<ExceptionFriendlyMessage>(
                () => Validator.ThrowExceptionFriendlyMessageIfAnyErrors(errors));
            Assert.Contains("Error 1", ex.Message);
            Assert.Contains("Error 2", ex.Message);
        }

        [Fact]
        public void ThrowExceptionFriendlyMessageIfAnyErrors_NoErrors_DoesNotThrow()
        {
            var ex = Record.Exception(
                () => Validator.ThrowExceptionFriendlyMessageIfAnyErrors(new List<string>()));
            Assert.Null(ex);
        }

        [Fact]
        public void ThrowExceptionFriendlyMessageIfAnyErrors_Null_DoesNotThrow()
        {
            var ex = Record.Exception(
                () => Validator.ThrowExceptionFriendlyMessageIfAnyErrors(null));
            Assert.Null(ex);
        }

        // GetValidationResults / ValidateAndThrow (data annotations)

        private class AnnotatedModel
        {
            [System.ComponentModel.DataAnnotations.Required]
            public string Name { get; set; }
        }

        [Fact]
        public void GetValidationResults_InvalidModel_ReturnsResults()
        {
            var results = Validator.GetValidationResults(new AnnotatedModel { Name = null });
            Assert.NotEmpty(results);
        }

        [Fact]
        public void GetValidationResults_ValidModel_ReturnsEmpty()
        {
            var results = Validator.GetValidationResults(new AnnotatedModel { Name = "ok" });
            Assert.Empty(results);
        }

        [Fact]
        public void ValidateAndThrow_InvalidModel_ThrowsExceptionFriendlyMessage()
        {
            Assert.Throws<ExceptionFriendlyMessage>(
                () => Validator.ValidateAndThrow(new AnnotatedModel { Name = null }));
        }

        [Fact]
        public void ValidateAndThrow_ValidModel_DoesNotThrow()
        {
            var ex = Record.Exception(
                () => Validator.ValidateAndThrow(new AnnotatedModel { Name = "ok" }));
            Assert.Null(ex);
        }

        // AndAddToErrorList accumulation across multiple validations

        [Fact]
        public void AndAddToErrorList_MultipleFailures_AccumulateInOrder()
        {
            var errors = new List<string>();
            Validator.ValidateStringAndAddToErrorList(null, "Name", required: true, errors);
            Validator.ValidateNumberAndAddToErrorList((decimal?)null, "Amount", required: true, errors);
            Validator.ValidateEmailAndAddToErrorList("bad", "Email", required: true, errors);

            Assert.Equal(3, errors.Count);
            Assert.Equal(Validator.Messages.Required("Name"), errors[0]);
            Assert.Equal(Validator.Messages.Required("Amount"), errors[1]);
            Assert.Equal(Validator.Messages.EmailInvalid("Email"), errors[2]);
        }
    }
}
