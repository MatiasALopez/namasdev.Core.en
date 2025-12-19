using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

using namasdev.Core.Types;
using namasdev.Core.Exceptions;
using namasdev.Core.Reflection;

namespace namasdev.Core.Validation
{
    public class Validator
    {
        public class Messages
        {
            public class Formats
            {
                public const string INVALID = "{0} is not in the valid format.";
                public const string TYPE_INVALID = "{0} is not a valid {1} value.";
                public const string MUST_BE_EMPTY = "{0} must be empty.";
                public const string LIST_NOT_EMPTY = "{0} must contain at least one valid element.";
                public const string REQUIRED = "{0} is required.";
                public const string ENTITY_NOT_FOUND = "{0} not found ({1}).";
                public const string ENTITY_DELETED = "{0} was already deleted ({2}).";
                public const string TEXT_LENGTH_MIN = "{0} must be {1} characters long al least.";
                public const string TEXT_LENGTH_MAX = "{0} must be {1} characters long at most.";
                public const string TEXT_LENGTH_RANGE = "{0} must be between {1} and {2} characters long.";
                public const string TEXT_EXACT_LENGTH = "{0} must be exactly {1} characters long.";
                public const string EMAIL_INVALID = "{0} is not a valid email address.";
                public const string IP_INVALID = "{0} is not a valid IP address";
                public const string INTEGER_INVALID = "{0} must be an integer number.";
                public const string SHORT_INVALID = "{0} must be a short integer number.";
                public const string LONG_INVALID = "{0} must be a long integer number.";
                public const string NUMBER_INVALID = "{0} must be a number.";
                public const string NUMBER_VALUE_MIN = "{0} must be a number greater than {1}.";
                public const string NUMBER_VALUE_MAX = "{0} must be a number lower than {1}.";
                public const string NUMBER_RANGE = "{0} must be a number between {1} and {2}.";
                public const string DATES_INVALID_RANGE = "Invalid date range ({0} - {1}).";
                public const string DATES_MONTH_COUNT_MAX = "Date range cannot exceed {0} months.";
                public const string DATE_TIME_INVALID = "{0} must be a valid date/time.";
                public const string DATE_TIME_MIN = "{0} must be a date/time greater than {2}.";
                public const string DATE_TIME_MAX = "{0} must be a date/time lower than {2}.";
                public const string DATE_TIME_RANGE = "{0} must be a date/time between {2} and {3}.";
                public const string DATE_INVALID = "{0} must be a valid date.";
                public const string DATE_MIN = "{0} must be a date greater than {2}.";
                public const string DATE_MAX = "{0} must be a date lower than {2}.";
                public const string DATE_RANGE = "{0} must be a date between {2} and {3}.";
                public const string TIME_INVALID = "{0} must be a valid time.";
                public const string TIME_RANGE_INVALID = "{0} is not a valid time range ({1} - {2}).";
                public const string TIME_RANGE_MIN = "{0} must be a time range of {1} at least.";
                public const string TIME_RANGE_MAX = "{0} must be a time range of {1} at most.";
                public const string TIME_RANGE_RANGE = "{0} must be a time range between {1} and {2}.";
                public const string TIME_RANGE_EXACT = "{0} must be a time range of {1}.";
                public const string BOOLEAN_INVALID = "{0} must be a boolean.";
                public const string FILE_EXTENSION_INVALID = "{0} has an invalid file extension. Valid extensions: {1}.";
            }

            public static string Invalid(string name)
            {
                return string.Format(Formats.INVALID, name);
            }

            public static string TypeInvalid(string name, string typeName)
            {
                return string.Format(Formats.TYPE_INVALID, name, typeName);
            }

            public static string MustBeEmpty(string name)
            {
                return String.Format(Formats.MUST_BE_EMPTY, name);
            }

            public static string ListNotEmpty(string name)
            {
                return String.Format(Formats.LIST_NOT_EMPTY, name);
            }

            public static string Required(string name)
            {
                return String.Format(Formats.REQUIRED, name);
            }

            public static string EntityNotFound(string entity, object valueBusqueda)
            {
                return String.Format(Formats.ENTITY_NOT_FOUND, entity, Convert.ToString(valueBusqueda));
            }

            public static string EntityDeleted(string entity, object valueBusqueda,
                bool nameEntityEsFemenino = false)
            {
                return String.Format(Formats.ENTITY_DELETED, entity, Convert.ToString(valueBusqueda));
            }

            public static string TextLengthMin(string name, int minLength)
            {
                return String.Format(Formats.TEXT_LENGTH_MIN, name, minLength);
            }

            public static string TextLengthMax(string name, int maxLength)
            {
                return String.Format(Formats.TEXT_LENGTH_MAX, name, maxLength);
            }

            public static string TextLengthRange(string name, int minLength, int maxLength)
            {
                return String.Format(Formats.TEXT_LENGTH_RANGE, name, minLength, maxLength);
            }

            public static string TextLengthExact(string name, int tamaño)
            {
                return String.Format(Formats.TEXT_EXACT_LENGTH, name, tamaño);
            }

            public static string EmailInvalid(string name)
            {
                return String.Format(Formats.EMAIL_INVALID, name);
            }

            public static string IPInvalid(string name)
            {
                return String.Format(Formats.IP_INVALID, name);
            }

            public static string IntegerInvalid(string name)
            {
                return String.Format(Formats.INTEGER_INVALID, name);
            }

            public static string ShortInvalid(string name)
            {
                return String.Format(Formats.SHORT_INVALID, name);
            }

            public static string LongInvalid(string name)
            {
                return String.Format(Formats.LONG_INVALID, name);
            }

            public static string NumberInvalid(string name)
            {
                return String.Format(Formats.NUMBER_INVALID, name);
            }

            public static string NumberValueMin(string name, long min)
            {
                return String.Format(Formats.NUMBER_VALUE_MIN, name, Formatter.Number(min));
            }

            public static string NumberValueMin(string name, decimal min, int decimalDigits)
            {
                return String.Format(Formats.NUMBER_VALUE_MIN, name, Formatter.Number(min, decimalDigits: decimalDigits));
            }

            public static string NumberValueMax(string name, long max)
            {
                return String.Format(Formats.NUMBER_VALUE_MAX, name, Formatter.Number(max));
            }

            public static string NumberValueMax(string name, decimal max, int decimalDigits)
            {
                return String.Format(Formats.NUMBER_VALUE_MAX, name, Formatter.Number(max, decimalDigits: decimalDigits));
            }

            public static string NumberRange(string name, long min, long max)
            {
                return String.Format(Messages.Formats.NUMBER_RANGE, name, Formatter.Number(min), Formatter.Number(max));
            }

            public static string NumberRange(string name, decimal min, decimal max, int decimalDigits)
            {
                return String.Format(Messages.Formats.NUMBER_RANGE, name, Formatter.Number(min, decimalDigits: decimalDigits), Formatter.Number(max, decimalDigits: decimalDigits));
            }

            public static string DatesInvalidRange(DateTime from, DateTime to,
                bool includeTime = false,
                DateFormat dateFormat = DateFormat.MDY)
            {
                Func<DateTime, string> formatter;
                if (includeTime)
                {
                    formatter = (f) => Formatter.DateTime(f, dateFormat);
                }
                else
                {
                    formatter = (f) => Formatter.Date(f, dateFormat);
                }

                return String.Format(Formats.DATES_INVALID_RANGE, formatter(from), formatter(to));
            }

            public static string DatesMonthCountMax(int monthCountMax)
            {
                return String.Format(Formats.DATES_MONTH_COUNT_MAX, monthCountMax);
            }

            public static string DateTimeInvalid(string name,
                bool includeTime = false)
            {
                return String.Format(Formats.DATE_TIME_INVALID, name);
            }

            public static string DateTimeMin(string name, DateTime min,
                bool includeTime = false,
                DateFormat dateFormat = DateFormat.MDY)
            {
                return String.Format(includeTime ? Formats.DATE_TIME_MIN : Formats.DATE_MIN, name, Formatter.DateTime(min, dateFormat));
            }

            public static string DateTimeMax(string name, DateTime max,
                bool includeTime = false,
                DateFormat dateFormat = DateFormat.MDY)
            {
                return String.Format(includeTime ? Formats.DATE_TIME_MAX : Formats.DATE_MAX, name, Formatter.DateTime(max, dateFormat));
            }

            public static string DateTimeRange(string name, DateTime from, DateTime to,
                bool includeTime = false,
                DateFormat dateFormat = DateFormat.MDY)
            {
                return String.Format(includeTime ? Formats.DATE_TIME_RANGE : Formats.DATE_RANGE, name, Formatter.DateTime(from, dateFormat), Formatter.DateTime(to, dateFormat));
            }

            public static string TimeInvalid(string name)
            {
                return String.Format(Formats.TIME_INVALID, name);
            }

            public static string TimeRangeInvalid(string name, TimeSpan from, TimeSpan to)
            {
                return String.Format(Formats.TIME_RANGE_INVALID, name, Formatter.Time(from), Formatter.Time(to));
            }

            public static string TimeRangeMin(string name, TimeSpan min)
            {
                return String.Format(Messages.Formats.TIME_RANGE_MIN, name, Formatter.Time(min));
            }

            public static string TimeRangeMax(string name, TimeSpan max)
            {
                return String.Format(Messages.Formats.TIME_RANGE_MAX, name, Formatter.Time(max));
            }

            public static string TimeRangeRange(string name, TimeSpan min, TimeSpan max)
            {
                return String.Format(Messages.Formats.TIME_RANGE_RANGE, name, Formatter.Time(min), Formatter.Time(max));
            }

            public static string TimeRangeExact(string name, TimeSpan value)
            {
                return String.Format(Messages.Formats.TIME_RANGE_EXACT, name, Formatter.Time(value));
            }

            public static string BooleanInvalid(string name)
            {
                return String.Format(Formats.BOOLEAN_INVALID, name);
            }

            public static string FileExtensionInvalid(string name, string validExtensions)
            {
                return String.Format(Formats.FILE_EXTENSION_INVALID, name, validExtensions);
            }
        }

        public static void ValidateAndThrow<TObject>(TObject obj,
            string messageHeader = null)
            where TObject : class
        {
            ValidateAndThrow<TObject, ExceptionFriendlyMessage>(obj, messageHeader: messageHeader);
        }

        public static void ValidateAndThrow<TObject, TException>(TObject obj,
            string messageHeader = null)
            where TObject : class
            where TException : Exception
        {
            var errors = GetValidationResults(obj);
            if (errors.Any())
            {
                throw ReflectionHelper.CreateInstance<TException>($"{(!String.IsNullOrWhiteSpace(messageHeader) ? $"{messageHeader}{Environment.NewLine}" : string.Empty)}{Formatter.List(errors, separator: Environment.NewLine)}");
            }
        }

        public static IEnumerable<ValidationResult> GetValidationResults<T>(T obj)
            where T : class
        {
            var res = new List<ValidationResult>();
            System.ComponentModel.DataAnnotations.Validator.TryValidateObject(obj, new ValidationContext(obj), res, true);
            return res;
        }

        public static void ValidateRequiredArgumentAndThrow(object value, string name)
        {
            ValidateRequiredAndThrow<ArgumentNullException>(value, name);
        }

        public static void ValidateRequiredListArgumentAndThrow<TEntity>(IEnumerable<TEntity> list, string name,
            bool validateNotEmpty = true,
            Func<TEntity, bool> validationItem = null)
        {
            ValidateListRequiredAndThrow<ArgumentNullException, TEntity>(list, name,
                validateNotEmpty: validateNotEmpty,
                validationItem: validationItem);
        }

        public static void ValidateRequiredAndThrow<TException>(object value, string message)
            where TException : Exception
        {
            if (value == null
                || (value is string && String.IsNullOrWhiteSpace((string)value)))
            {
                throw ReflectionHelper.CreateInstance<TException>(message);
            }
        }

        public static void ValidateListRequiredAndThrow<TException, TEntity>(IEnumerable<TEntity> list, string message,
            bool validateNotEmpty = true,
            Func<TEntity, bool> validationItem = null)
            where TException : Exception
        {
            validationItem = validationItem ?? (e => true);

            if (list == null
                || (validateNotEmpty && !list.Any(i => validationItem(i))))
            {
                throw ReflectionHelper.CreateInstance<TException>(message);
            }
        }

        public static void ValidateEntityNotNullAndThrow(object entity, object id, string description)
        {
            if (entity == null)
            {
                throw new Exception(Messages.EntityNotFound(description, id));
            }
        }

        public static bool ValidateFile(IO.File file, bool required,
            out string message,
            string description = null,
            string extensionList = null)
        {
            return ValidateFile(file, required,
                out message,
                description: description, 
                extensionList: GetFileExtensionList(extensionList));
        }

        public static bool ValidateFileAndAddToErrorList(IO.File file, bool required, 
            List<string> errors,
            string description = null,
            string extensionList = null)
        {
            return ValidateAndAddToErrorList(
                () =>
                {
                    ValidateFile(file, required,
                        out string errorMessage,
                        description: description,
                        extensionList: extensionList);

                    return errorMessage;
                },
                errors);
        }

        public static bool ValidateFile(IO.File file, bool required,
            out string message,
            string description = null,
            IEnumerable<string> extensionList = null)
        {
            message = null;

            if (file == null || file.Content == null || file.Content.Length == 0)
            {
                if (required)
                {
                    message = Messages.Required(GetFileDescription(file, description));
                    return false;
                }
                else
                {
                    return true;
                }
            }

            if (extensionList != null)
            {
                return ValidateFileExtension(file.Name, extensionList,
                    out message,
                    description: description);
            }

            return true;
        }

        public static bool ValidateFileAndAddToErrorList(IO.File file, bool required, 
            List<string> errors,
            string description = null,
            IEnumerable<string> extensionList = null)
        {
            return ValidateAndAddToErrorList(
                () =>
                {
                    ValidateFile(file, required,
                        out string errorMessage,
                        description: description,
                        extensionList: extensionList);

                    return errorMessage;
                },
                errors);
        }

        public static bool ValidateFileExtension(string fileName, string extensionList,
            out string message,
            string description = null)
        {
            return ValidateFileExtension(fileName, GetFileExtensionList(extensionList),
                out message,
                description: description);
        }

        public static bool ValidateFileExtensionAndAddToErrorList(string fileName, string extensionList,
            List<string> errors,
            string description = null)
        {
            return ValidateAndAddToErrorList(
                () =>
                {
                    ValidateFileExtension(fileName, GetFileExtensionList(extensionList),
                        out string errorMessage,
                        description: description);

                    return errorMessage;
                },
                errors);
        }

        public static bool ValidateFileExtension(string fileName, IEnumerable<string> extensionList,
            out string message,
            string description = null)
        {
            if (!IsValidFileExtension(fileName, extensionList))
            {
                message = Messages.FileExtensionInvalid(GetFileDescription(fileName, description), Formatter.List(extensionList, ", "));
                return false;
            }

            message = null;
            return true;
        }

        public static bool ValidateFileExtensionAndAddToErrorList(string fileName, IEnumerable<string> extensionList, 
            List<string> errors,
            string description = null)
        {
            return ValidateAndAddToErrorList(
                () =>
                {
                    ValidateFileExtension(fileName, extensionList,
                        out string errorMessage,
                        description: description);

                    return errorMessage;
                },
                errors);
        }

        public static bool IsValidFileExtension(string fileName, string extensionList)
        {
            return IsValidFileExtension(fileName, GetFileExtensionList(extensionList));
        }

        public static bool IsValidFileExtension(string fileName, IEnumerable<string> extensionList)
        {
            ValidateRequiredArgumentAndThrow(extensionList, nameof(extensionList));

            return extensionList.Contains(Path.GetExtension(fileName).ToLower());
        }

        private static string GetFileDescription(IO.File file, string description)
        {
            return GetFileDescription(file?.Name, description);

        }
        
        private static string GetFileDescription(string fileName, string description)
        {
            if (!string.IsNullOrWhiteSpace(description))
            {
                return description;
            }
            else if (!string.IsNullOrWhiteSpace(fileName))
            {
                return Path.GetFileName(fileName);
            }

            return "File";
        }

        private static string[] GetFileExtensionList(string extensionList)
        {
            return 
                string.IsNullOrWhiteSpace(extensionList)
                ? null
                : extensionList.Split(',');
        }

        public static bool ValidateString(
            string value, string name, bool required,
            out string errorMessage,
            int? maxLength = null, int? minLength = null, int? exactLength = null, 
            string regEx = null)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                if (required)
                {
                    errorMessage = Messages.Required(name);
                    return false;
                }
            }
            else
            {
                if (exactLength.HasValue)
                {
                    if (value.Length != exactLength.Value)
                    {
                        errorMessage = Messages.TextLengthExact(name, exactLength.Value);
                        return false;
                    }
                }
                else if (minLength.HasValue && maxLength.HasValue)
                {
                    if (value.Length < minLength.Value
                        || value.Length > maxLength.Value)
                    {
                        errorMessage = Messages.TextLengthRange(name, minLength.Value, maxLength.Value);
                        return false;
                    }
                }
                else if (minLength.HasValue)
                {
                    if (value.Length < minLength.Value)
                    {
                        errorMessage = Messages.TextLengthMin(name, minLength.Value);
                        return false;
                    }
                }
                else if (maxLength.HasValue)
                {
                    if (value.Length > maxLength.Value)
                    {
                        errorMessage = Messages.TextLengthMax(name, maxLength.Value);
                        return false;
                    }
                }
                
                if (!String.IsNullOrWhiteSpace(regEx))
                {
                    if (!Regex.IsMatch(value, regEx))
                    {
                        errorMessage = Messages.Invalid(name);
                        return false;
                    }
                }
            }

            errorMessage = null;
            return true;
        }

        public static bool ValidateStringAndAddToErrorList(
            string value, string name, bool required,
            List<string> errors,
            int? maxLength = null, int? minLength = null, int? exactLength = null, 
            string regEx = null)
        {
            return ValidateAndAddToErrorList(
                () =>
                {
                    ValidateString(value, name, required,
                        out string errorMessage,
                        maxLength: maxLength,
                        minLength: minLength,
                        exactLength: exactLength,
                        regEx: regEx);

                    return errorMessage;
                },
                errors);
        }

        public static bool ValidateDateRange(DateTime dateFrom, DateTime dateTo,
            out string errorMessage,
            int? monthCountMax = null,
            bool includeTime = false,
            DateFormat dateFormat = DateFormat.MDY)
        {
            if (dateFrom > dateTo)
            {
                errorMessage = Messages.DatesInvalidRange(dateFrom, dateTo, 
                    includeTime: includeTime, 
                    dateFormat: dateFormat);
                return false;
            }
            else
            {
                if (monthCountMax.HasValue)
                {
                    var monthCount = new MonthOfYear(dateFrom)
                        .DiffInMonths(new MonthOfYear(dateTo)) + 1;

                    if (monthCount > monthCountMax)
                    {
                        errorMessage = Messages.DatesMonthCountMax(monthCountMax.Value);
                        return false;
                    }
                }
            }

            errorMessage = null;
            return true;
        }

        public static bool ValidateDateRangeAndAddToErrorList(DateTime dateFrom, DateTime dateTo,
            List<string> errors,
            int? monthCountMax = null,
            bool includeTime = false)
        {
            return ValidateAndAddToErrorList(
                () =>
                {
                    ValidateDateRange(dateFrom, dateTo, 
                        out string errorMessage, 
                        monthCountMax: monthCountMax,
                        includeTime: includeTime);

                    return errorMessage;
                },
                errors);
        }

        public static bool ValidateEmail(string email, string name, bool required,
            out string errorMessage)
        {
            if (!ValidateString(email, name, required, 
                out errorMessage))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                return true;
            }

            try
            {
                new MailAddress(email);

                errorMessage = null;
                return true;
            }
            catch (Exception)
            {
                errorMessage = Messages.EmailInvalid(name);
                return false;
            }
        }

        public static bool ValidateEmailAndAddToErrorList(string email, string name, bool required,
            List<string> errors)
        {
            return ValidateAndAddToErrorList(
                () =>
                {
                    ValidateEmail(email, name, required,
                        out string errorMessage);

                    return errorMessage;
                },
                errors);
        }

        public static bool ValidateIP(string ipAddress, string name,
            out string errorMessage)
        {
            IPAddress oIPAddress;
            if (!IPAddress.TryParse(ipAddress, out oIPAddress)
                || !String.Equals(ipAddress, oIPAddress.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                errorMessage = Messages.IPInvalid(name);
                return false;
            }

            errorMessage = null;
            return true;
        }

        public static bool ValidateIPAndAddToErrorList(string ipAddress, string name,
            List<string> errors)
        {
            return ValidateAndAddToErrorList(
                () =>
                {
                    ValidateIP(ipAddress, name,
                        out string errorMessage);

                    return errorMessage;
                },
                errors);
        }

        public static bool ValidateNumberAndAddToErrorList(decimal? value, string name, bool required,
            List<string> errors,
            decimal? minValue = null, decimal? maxValue = null,
            int decimalDigits = 2)
        {
            return ValidateAndAddToErrorList(
                () =>
                {
                    ValidateNumber(value, name, required,
                        out string errorMessage,
                        minValue: minValue, maxValue: maxValue,
                        decimalDigits: decimalDigits);

                    return errorMessage;
                },
                errors);
        }

        public static bool ValidateNumber(decimal? value, string name, bool required,
            out string errorMessage,
            decimal? minValue = null, decimal? maxValue = null,
            int decimalDigits = 2)
        {
            if (!value.HasValue)
            {
                if (required)
                {
                    errorMessage = Messages.Required(name);
                    return false;
                }
            }
            else
            {
                if (minValue.HasValue && maxValue.HasValue)
                {
                    if (value < minValue
                        || value > maxValue)
                    {
                        errorMessage = Messages.NumberRange(name, minValue.Value, maxValue.Value, decimalDigits);
                        return false;
                    }
                }
                else
                {
                    if (minValue.HasValue
                        && value < minValue)
                    {
                        errorMessage = Messages.NumberValueMin(name, minValue.Value, decimalDigits: decimalDigits);
                        return false;
                    }

                    if (maxValue.HasValue
                        && value > maxValue)
                    {
                        errorMessage = Messages.NumberValueMax(name, maxValue.Value, decimalDigits: decimalDigits);
                        return false;
                    }
                }
            }

            errorMessage = null;
            return true;
        }

        public static bool ValidateNumberAndAddToErrorList(double? value, string name, bool required,
            List<string> errors,
            double? minValue = null, double? maxValue = null,
            int decimalDigits = 2)
        {
            return ValidateNumberAndAddToErrorList((decimal?)value, name, required,
                errors,
                minValue: (decimal?)minValue, maxValue: (decimal?)maxValue,
                decimalDigits: decimalDigits);
        }

        public static bool ValidateNumber(double? value, string name, bool required,
            out string errorMessage,
            double? minValue = null, double? maxValue = null,
            int decimalDigits = 2)
        {
            return ValidateNumber((decimal?)value, name, required,
                out errorMessage,
                minValue: (decimal?)minValue, maxValue: (decimal?)maxValue,
                decimalDigits: decimalDigits);
        }

        public static bool ValidateNumberAndAddToErrorList(int? value, string name, bool required,
            List<string> errors,
            int? minValue = null, int? maxValue = null)
        {
            return ValidateNumberAndAddToErrorList((long?)value, name, required,
                errors,
                minValue: minValue, maxValue: maxValue);
        }

        public static bool ValidateNumber(int? value, string name, bool required,
            out string errorMessage,
            int? minValue = null, int? maxValue = null)
        {
            return ValidateNumber((long?)value, name, required,
                out errorMessage,
                minValue: minValue, maxValue: maxValue);
        }

        public static bool ValidateNumberAndAddToErrorList(short? value, string name, bool required,
            List<string> errors,
            short? minValue = null, short? maxValue = null)
        {
            return ValidateNumberAndAddToErrorList((long?)value, name, required,
                errors,
                minValue: minValue, maxValue: maxValue);
        }

        public static bool ValidateNumber(short? value, string name, bool required,
            out string errorMessage,
            short? minValue = null, short? maxValue = null)
        {
            return ValidateNumber((long?)value, name, required,
                out errorMessage,
                minValue: minValue, maxValue: maxValue);
        }

        public static bool ValidateNumberAndAddToErrorList(long? value, string name, bool required,
            List<string> errors,
            long? minValue = null, long? maxValue = null)
        {
            return ValidateNumberAndAddToErrorList(value, name, required,
                errors,
                minValue: minValue, maxValue: maxValue);
        }

        public static bool ValidateNumber(long? value, string name, bool required,
            out string errorMessage,
            long? minValue = null, long? maxValue = null,
            int decimalDigits = 2)
        {
            if (!value.HasValue)
            {
                if (required)
                {
                    errorMessage = Messages.Required(name);
                    return false;
                }
            }
            else
            {
                if (minValue.HasValue && maxValue.HasValue)
                {
                    if (value < minValue
                        || value > maxValue)
                    {
                        errorMessage = Messages.NumberRange(name, minValue.Value, maxValue.Value);
                        return false;
                    }
                }
                else
                {
                    if (minValue.HasValue
                        && value < minValue)
                    {
                        errorMessage = Messages.NumberValueMin(name, minValue.Value);
                        return false;
                    }

                    if (maxValue.HasValue
                        && value > maxValue)
                    {
                        errorMessage = Messages.NumberValueMax(name, maxValue.Value, decimalDigits: decimalDigits);
                        return false;
                    }
                }
            }

            errorMessage = null;
            return true;
        }

        public static bool ValidateDate(
            DateTime? date, string name, bool required,
            out string errorMessage,
            DateTime? minValue = null, DateTime? maxValue = null,
            bool includeTime = false,
            DateFormat dateFormat = DateFormat.MDY)
        {
            if (!date.HasValue)
            {
                if (required)
                {
                    errorMessage = Messages.Required(name);
                    return false;
                }
            }
            else
            {
                if (!includeTime)
                {
                    date = date.Value.Date;
                    minValue = minValue?.Date;
                    maxValue = maxValue?.Date;
                }

                if (minValue.HasValue && maxValue.HasValue)
                {
                    if (date < minValue
                        || date > maxValue)
                    {
                        errorMessage = Messages.DateTimeRange(name, minValue.Value, maxValue.Value, 
                            includeTime: includeTime,
                            dateFormat: dateFormat);
                        return false;
                    }
                }
                else if (minValue.HasValue)
                {
                    if (date < minValue)
                    {
                        errorMessage = Messages.DateTimeMin(name, minValue.Value,
                            includeTime: includeTime,
                            dateFormat: dateFormat);
                        return false;
                    }
                }
                else if (maxValue.HasValue)
                {
                    if (date > maxValue)
                    {
                        errorMessage = Messages.DateTimeMax(name, maxValue.Value,
                            includeTime: includeTime,
                            dateFormat: dateFormat);
                        return false;
                    }
                }
            }

            errorMessage = null;
            return true;
        }

        public static bool ValidateDateAndAddToErrorList(
            DateTime? date, string name, bool required,
            List<string> errors,
            DateTime? minValue = null, DateTime? maxValue = null,
            bool includeTime = false)
        {
            return ValidateAndAddToErrorList(
                () =>
                {
                    ValidateDate(date, name, required,
                        out string errorMessage,
                        minValue: minValue, maxValue: maxValue,
                        includeTime: includeTime);
                    
                    return errorMessage;
                },
                errors);
        }

        public static bool ValidateTimeRange(
            TimeSpan from, TimeSpan to, string name,
            out string errorMessage,
            TimeSpan? minValue = null, TimeSpan? maxValue = null,
            TimeSpan? exactValue = null)
        {
            if (from > to)
            {
                errorMessage = Messages.TimeRangeInvalid(name, from, to);
                return false;
            }

            if (minValue.HasValue && minValue == maxValue)
            {
                exactValue = minValue.Value;
            }

            var diffTime = to - from;
            if (exactValue.HasValue)
            {
                if (diffTime != exactValue.Value)
                {
                    errorMessage = Messages.TimeRangeExact(name, exactValue.Value);
                    return false;
                }
            }
            else if (minValue.HasValue && maxValue.HasValue)
            {
                if (diffTime < minValue
                    || diffTime > maxValue)
                {
                    errorMessage = Messages.TimeRangeRange(name, minValue.Value, maxValue.Value);
                    return false;
                }
            }
            else if (minValue.HasValue)
            {
                if (diffTime < minValue.Value)
                {
                    errorMessage = Messages.TimeRangeMin(name, minValue.Value);
                    return false;
                }
            }
            else if (maxValue.HasValue)
            {
                if (diffTime > maxValue.Value)
                {
                    errorMessage = Messages.TimeRangeMax(name, maxValue.Value);
                    return false;
                }
            }

            errorMessage = null;
            return true;
        }

        public static bool ValidateTimeRangeAndAddToErrorList(
            TimeSpan from, TimeSpan to, string name,
            List<string> errors,
            TimeSpan? minValue = null, TimeSpan? maxValue = null)
        {
            return ValidateAndAddToErrorList(
                () =>
                {
                    ValidateTimeRange(from, to, name,
                        out string errorMessage,
                        minValue: minValue, maxValue: maxValue);

                    return errorMessage;
                },
                errors);
        }

        private static bool ValidateAndAddToErrorList(Func<string> validation, List<string> errors)
        {
            ValidateRequiredListArgumentAndThrow(errors, nameof(errors), validateNotEmpty: false);

            string errorMessage = validation();
            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                errors.Add(errorMessage);
                return false;
            }

            return true;
        }

        public static void ThrowExceptionFriendlyMessageIfAnyErrors(List<string> errors)
        {
            if (errors == null)
            {
                return;
            }

            if (errors.Any())
            {
                throw new ExceptionFriendlyMessage(Formatter.List(errors, Environment.NewLine));
            }
        }
    }
}
