using System;
using System.Linq;

namespace FusionCore.FusionReflection
{
    public static class ExtensaoAttribute
    {
        public static TValue GetAttributeValue<TAttribute, TValue>(
            this Type type,
            Func<TAttribute, TValue> valueSelector)
            where TAttribute : Attribute
        {
            var att = type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;

            return att != null ? valueSelector(att) : default(TValue);
        }
    }
}