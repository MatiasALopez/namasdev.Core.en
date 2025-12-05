using System.Collections.Generic;
using System.Dynamic;

namespace namasdev.Core.Types
{
    public class ExpandoObjectHelper
    {
        public static ExpandoObject CreateFromDictionary<TValue>(Dictionary<string, TValue> dictionary)
        {
            var expObj = new ExpandoObject();
            var expDic = (IDictionary<string, object>)expObj;
            foreach (var item in dictionary)
            {
                expDic.Add(item.Key, item.Value);
            }
            return expObj;
        }

        public static void Update<TValue>(ExpandoObject source, Dictionary<string, TValue> newValues)
        {
            var expDic = (IDictionary<string, object>)source;
            foreach (var item in newValues)
            {
                expDic.AddOrSetValue(item.Key, item.Value);
            }
        }
    }
}
