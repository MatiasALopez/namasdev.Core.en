using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using namasdev.Core.Types;

namespace namasdev.Core.IO
{
    public abstract class TextFileRecord
    {
        private List<string> _errors;

        public TextFileRecord(int lineNumber,
            bool includePositionInError = false)
        {
            LineNumber = lineNumber;
            IncludePositionInError = includePositionInError;

            _errors = new List<string>();
        }

        public int LineNumber { get; private set; }
        public IEnumerable<string> Errors
        {
            get { return _errors.AsReadOnly(); }
        }
        private bool IncludePositionInError { get; set; }

        public bool IsValid
        {
            get { return _errors.Count == 0; }
        }

        protected bool HasAnyData(string[] dataValues)
        {
            return dataValues != null
                && dataValues.Any()
                && dataValues.Any(d => !String.IsNullOrWhiteSpace(d));
        }

        protected string GetString(string[] dataValues, int position, string dataDescription,
            bool isRequired = true, int? maxLength = null,
            bool convertEmptyToNull = true)
        {
            string value = GetValueInPosition(dataValues, position, dataDescription, 
                isRequired: isRequired);

            string errorMessage;
            if (!Validation.Validator.ValidateString(value, dataDescription, isRequired, 
                out errorMessage,
                maxLength: maxLength))
            {
                AddError(position, errorMessage);
            }

            return String.IsNullOrWhiteSpace(value) && convertEmptyToNull
                ? null
                : value;
        }

        protected int? GetInt(string[] dataValues, int position, string dataDescription, 
            bool isRequired = true)
        {
            var value = GetString(dataValues, position, dataDescription,
                isRequired: isRequired);

            if (!String.IsNullOrWhiteSpace(value))
            {
                try
                {
                    return int.Parse(value);
                }
                catch (Exception)
                {
                    AddError(position, Validation.Validator.Messages.IntegerInvalid(dataDescription));
                }
            }

            return null;
        }

        protected short? GetShort(string[] dataValues, int position, string dataDescription, 
            bool isRequired = true)
        {
            var value = GetString(dataValues, position, dataDescription, 
                isRequired: isRequired);

            if (!String.IsNullOrWhiteSpace(value))
            {
                try
                {
                    return short.Parse(value);
                }
                catch (Exception)
                {
                    AddError(position, Validation.Validator.Messages.ShortInvalid(dataDescription));
                }
            }

            return null;
        }

        protected long? GetLong(string[] dataValues, int position, string dataDescription, 
            bool isRequired = true)
        {
            var value = GetString(dataValues, position, dataDescription, 
                isRequired: isRequired);

            if (!String.IsNullOrWhiteSpace(value))
            {
                try
                {
                    return long.Parse(value);
                }
                catch (Exception)
                {
                    AddError(position, Validation.Validator.Messages.LongInvalid(dataDescription));
                }
            }

            return null;
        }

        protected double? GetDouble(string[] dataValues, int position, string dataDescription,
            bool isRequired = true)
        {
            var value = GetString(dataValues, position, dataDescription, 
                isRequired: isRequired);

            if (!String.IsNullOrWhiteSpace(value))
            {
                try
                {
                    return double.Parse(value);
                }
                catch (Exception)
                {
                    AddError(position, Validation.Validator.Messages.NumberInvalid(dataDescription));
                }
            }

            return null;
        }

        protected decimal? GetDecimal(string[] dataValues, int position, string dataDescription, 
            bool isRequired = true)
        {
            var value = GetString(dataValues, position, dataDescription,
                isRequired: isRequired);

            if (!String.IsNullOrWhiteSpace(value))
            {
                try
                {
                    return decimal.Parse(value);
                }
                catch (Exception)
                {
                    AddError(position, Validation.Validator.Messages.NumberInvalid(dataDescription));
                }
            }

            return null;
        }

        protected DateTime? GetDateTime(string[] dataValues, int position, string dataDescription,
            bool isRequired = true, 
            string dateFormat = null)
        {
            var value = GetString(dataValues, position, dataDescription, 
                isRequired: isRequired);

            if (!String.IsNullOrWhiteSpace(value))
            {
                try
                {
                    return String.IsNullOrWhiteSpace(dateFormat)
                        ? DateTime.Parse(value)
                        : DateTime.ParseExact(value, dateFormat, CultureInfo.CurrentCulture);
                }
                catch (Exception)
                {
                    AddError(position, Validation.Validator.Messages.DateTimeInvalid(dataDescription));
                }
            }

            return null;
        }

        protected TimeSpan? GetTimeSpan(string[] dataValues, int position, string dataDescription,
            bool isRequired = true, string formato = null)
        {
            var value = GetString(dataValues, position, dataDescription, 
                isRequired: isRequired);

            if (!String.IsNullOrWhiteSpace(value))
            {
                try
                {
                    return String.IsNullOrWhiteSpace(formato)
                        ? TimeSpan.Parse(value)
                        : TimeSpan.ParseExact(value, formato, CultureInfo.CurrentCulture);
                }
                catch (Exception)
                {
                    AddError(position, Validation.Validator.Messages.TimeInvalid(dataDescription));
                }
            }

            return null;
        }

        protected bool? GetBoolean(string[] dataValues, int position, string dataDescription, 
            bool isRequired = true)
        {
            var value = GetString(dataValues, position, dataDescription, 
                isRequired: isRequired);

            if (!String.IsNullOrWhiteSpace(value))
            {
                try
                {
                    return bool.Parse(value);
                }
                catch (Exception)
                {
                    try
                    {
                        return string.Equals(value, Formatter.YES, StringComparison.CurrentCultureIgnoreCase);
                    }
                    catch (Exception)
                    {
                        AddError(position, Validation.Validator.Messages.BooleanInvalid(dataDescription));
                    }
                }
            }

            return null;
        }

        private string GetValueInPosition(string[] dataValues, int position, string description, 
            bool isRequired = true)
        {
            int index = position - 1;
            if (position < 1 || position > dataValues.Length)
            {
                AddError(position, $"Invalid position (position: {position}, max. position: {dataValues.Length}).");
                return null;
            }

            string value = dataValues[index];

            if (String.IsNullOrWhiteSpace(value))
            {
                if (isRequired)
                {
                    AddError(position, Validation.Validator.Messages.Required(description));
                }
            }

            return value;
        }

        protected void AddError(int position, string error)
        {
            _errors.Add($"{(IncludePositionInError ? $"[Position {position}] " : String.Empty)}{error}");
        }
    }
}
