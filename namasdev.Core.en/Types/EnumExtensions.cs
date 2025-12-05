using System;
using System.ComponentModel;

using namasdev.Core.Reflection;

namespace namasdev.Core.Types
{
    public static class EnumExtensions
    {
        public static string Description(this Enum value)
        {
            if (value == null)
            {
                return String.Empty;
            }

            DescriptionAttribute attribute = null;
            if (IsValid(value))
            {
                attribute = ReflectionHelper.GetFieldAttribute<DescriptionAttribute>(value.GetType(), value.ToString());
            }

            return attribute != null
                ? attribute.Description
                : value.ToString();
        }

        public static bool IsValid(this Enum value)
        {
            if (value == null)
            {
                return true;
            }

            //  In order to validate we cast the value to a number;
            //  if it works we assume the value is NOT a valid Enum option
            return !long.TryParse(value.ToString(), out _);
        }
    }
}
