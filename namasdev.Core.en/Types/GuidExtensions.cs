using System;

namespace namasdev.Core.Types
{
    public static class GuidExtensions
    {
        public static Guid? ValueOrNullIfEmpty(this Guid value)
        {
            return value != Guid.Empty
                ? value
                : (Guid?)null;
        }
    }
}
