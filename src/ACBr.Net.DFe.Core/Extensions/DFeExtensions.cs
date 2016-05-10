using System;
using System.Linq;
using System.Reflection;
using ACBr.Net.DFe.Core.Attributes;
using ACBr.Net.DFe.Core.Interfaces;
using ExtraConstraints;

namespace ACBr.Net.DFe.Core.Extensions
{
    internal static class DFeExtensions
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

	    internal static IDFeElement GetTag(this PropertyInfo prop)
	    {
			return prop.HasAttribute<DFeElementAttribute>()
					? (IDFeElement)prop.GetAttribute<DFeElementAttribute>()
					: prop.GetAttribute<DFeAttributeAttribute>();
		}

	    internal static TDelegate ToDelegate<[DelegateConstraint]TDelegate>(this MethodInfo method, object item) where TDelegate : class
	    {
			return Delegate.CreateDelegate(typeof(TDelegate), item, method) as TDelegate;
		}

		internal static Func<string> GetSerializer<T>(this T item, PropertyInfo prop) where T : class
		{
			var method = item.GetType().GetMethod($"Serialize{prop.Name}", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			return method.ToDelegate<Func<string>>(item);
		}

		internal static Func<string, object> GetDeserializer<T>(this T item, PropertyInfo prop) where T : class
		{
			var method = item.GetType().GetMethod($"Deserialize{prop.Name}", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			return method.ToDelegate<Func<string, object>>(item);
		}
	}
}