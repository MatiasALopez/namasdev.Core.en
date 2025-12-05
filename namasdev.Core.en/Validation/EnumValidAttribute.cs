using System;
using System.ComponentModel.DataAnnotations;

using namasdev.Core.Types;

namespace namasdev.Core.Validation
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class EnumValidAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            if (!(value is Enum))
            {
                return false;
            }

            return ((Enum)value).IsValid();
        }
    }
}
