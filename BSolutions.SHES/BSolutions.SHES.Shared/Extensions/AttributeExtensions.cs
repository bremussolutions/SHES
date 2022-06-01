using System;
using System.Linq;

namespace BSolutions.SHES.Shared.Extensions
{
    public static class AttributeExtensions
    {
        public static TValue GetAttributeValue<TAttribute, TValue>(this Type type, Func<TAttribute, TValue> valueSelector) where TAttribute : Attribute
        {
            var attr = type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;

            if (attr != null)
            {
                return valueSelector(attr);
            }

            return default(TValue);
        }
    }
}
