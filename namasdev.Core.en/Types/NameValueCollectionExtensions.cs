using System.Collections.Specialized;

using namasdev.Core.Validation;

namespace namasdev.Core.Types
{
    public static class NameValueCollectionExtensions
    {
        public static void AddOrReplace(this NameValueCollection collection, NameValueCollection newValues)
        {
            Validator.ValidateRequiredArgumentAndThrow(collection, nameof(collection));

            if (newValues == null)
            {
                return;
            }

            int count = newValues.Count;
            for (int i = 0; i < count; i++)
            {
                collection.Set(newValues.GetKey(i), newValues.Get(i));
            }
        }
    }
}
