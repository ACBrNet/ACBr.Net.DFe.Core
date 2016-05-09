using System;
using System.Linq;
using System.Reflection;

namespace ACBr.Net.DFe.Core.Internal
{
    internal static class AttributeExtensions
    {
        internal static TValue GetAttributeValue<TAttribute, TValue>(
            this ICustomAttributeProvider type,
            Func<TAttribute, TValue> valueSelector)
            where TAttribute : Attribute
        {
            var att = type.GetCustomAttributes(
                typeof(TAttribute), true
                ).FirstOrDefault() as TAttribute;

            return att != null ? valueSelector(att) : default(TValue);
        }

		internal static TAttribute GetAttribute<TAttribute>(this ICustomAttributeProvider type)
			where TAttribute : Attribute
		{
			var att = type.GetCustomAttributes(typeof(TAttribute), true).Cast<TAttribute>().FirstOrDefault();
			return att;
		}

		internal static TAttribute[] GetAttributes<TAttribute>(this ICustomAttributeProvider type)
            where TAttribute : Attribute
        {
            var att = type.GetCustomAttributes(typeof(TAttribute), true)
				.Cast<TAttribute>().ToArray();
            return att;
        }

        internal static bool HasAttribute<T>(this ICustomAttributeProvider provider) where T : Attribute
        {
            var atts = provider.GetCustomAttributes(typeof(T), true);
            return atts.Length > 0;
        }
	}
}