// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 05-10-2016
//
// Last Modified By : RFTD
// Last Modified On : 05-11-2016
// ***********************************************************************
// <copyright file="DFeExtensions.cs" company="ACBr.Net">
//		        		   The MIT License (MIT)
//	     		    Copyright (c) 2016 Grupo ACBr.Net
//
//	 Permission is hereby granted, free of charge, to any person obtaining 
// a copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the 
// Software is furnished to do so, subject to the following conditions:
//	 The above copyright notice and this permission notice shall be 
// included in all copies or substantial portions of the Software.
//	 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using ACBr.Net.Core.Exceptions;
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

		internal static TDelegate ToDelegate<[DelegateConstraint]TDelegate>(this MethodInfo method) where TDelegate : class
		{
			return Delegate.CreateDelegate(typeof(TDelegate), method) as TDelegate;
		}

		internal static TDelegate ToDelegate<[DelegateConstraint]TDelegate>(this MethodInfo method, object item) where TDelegate : class
	    {
			return Delegate.CreateDelegate(typeof(TDelegate), item, method) as TDelegate;
		}

		internal static Func<string> GetSerializer(this PropertyInfo prop, object item)
		{
			Guard.Against<ArgumentException>(prop.DeclaringType != item.GetType(), "O item informado não declara esta propriedade");
			var method = item.GetType().GetMethod($"Serialize{prop.Name}", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			return method.ToDelegate<Func<string>>(item);
		}

		internal static Func<string, object> GetDeserializer(this PropertyInfo prop, object item)
		{
			Guard.Against<ArgumentException>(prop.DeclaringType != item.GetType(), "O item informado não declara esta propriedade");
			var method = item.GetType().GetMethod($"Deserialize{prop.Name}", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			return method.ToDelegate<Func<string, object>>(item);
		}

	    internal static Func<object> GetCreate(this Type tipo)
	    {
			var method = tipo.GetMethod("Create", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
			return method.ToDelegate<Func<object>>();
		}

		internal static bool HasCreate(this Type tipo)
		{
			var method = tipo.GetMethod("Create", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
			return method != null;
		}

		public static IEnumerable<XElement> ElementsAnyNs(this XElement source, string name)
		{
			return source.Elements().Where(e => e.Name.LocalName == name);
		}
	}
}