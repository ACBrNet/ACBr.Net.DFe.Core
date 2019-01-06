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

using ACBr.Net.Core.Exceptions;
using ACBr.Net.DFe.Core.Attributes;
using System;
using System.Collections;
using System.Reflection;
using ACBr.Net.Core.Extensions;

namespace ACBr.Net.DFe.Core.Extensions
{
    internal static class DFeExtensions
    {
        public static DFeBaseAttribute GetTag(this PropertyInfo prop)
        {
            return prop.HasAttribute<DFeElementAttribute>()
                    ? (DFeBaseAttribute)prop.GetAttribute<DFeElementAttribute>()
                    : prop.GetAttribute<DFeAttributeAttribute>();
        }

        public static TDelegate ToDelegate<TDelegate>(this MethodInfo method) where TDelegate : Delegate
        {
            return Delegate.CreateDelegate(typeof(TDelegate), method) as TDelegate;
        }

        public static TDelegate ToDelegate<TDelegate>(this MethodInfo method, object item) where TDelegate : Delegate
        {
            return Delegate.CreateDelegate(typeof(TDelegate), item, method) as TDelegate;
        }

        public static Func<string> GetSerializer(this PropertyInfo prop, object item)
        {
            Guard.Against<ArgumentException>(prop.DeclaringType != item.GetType(), "O item informado não declara esta propriedade");

            var method = item.GetType().GetMethod($"Serialize{prop.Name}", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            return method.ToDelegate<Func<string>>(item);
        }

        public static string GetRootName(this Type prop, object item)
        {
            var method = item.GetType().GetMethod("GetRootName", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (method == null || method.ReturnType != typeof(string)) return string.Empty;

            var func = method.ToDelegate<Func<string>>(item);
            return func();
        }

        public static string[] GetRootNames(this Type prop)
        {
            var method = prop.GetMethod("GetRootNames", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            if (method == null || method.ReturnType != typeof(string[])) return new string[0];

            var func = method.ToDelegate<Func<string[]>>();
            return func();
        }

        public static Func<string, object> GetDeserializer(this PropertyInfo prop, object item)
        {
            Guard.Against<ArgumentException>(prop.DeclaringType != item.GetType(), "O item informado não declara esta propriedade");
            var method = item.GetType().GetMethod($"Deserialize{prop.Name}", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            return method.ToDelegate<Func<string, object>>(item);
        }

        public static object GetValueOrIndex(this PropertyInfo prop, object parent, int index)
        {
            var value = prop.GetValue(parent, null);
            if (index > -1)
            {
                value = ((IList)value)[index];
            }

            return value;
        }

        public static Func<object> GetCreate(this Type tipo)
        {
            var method = tipo.GetMethod("Create", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            return method.ToDelegate<Func<object>>();
        }

        public static bool HasCreate(this Type tipo)
        {
            var method = tipo.GetMethod("Create", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            return method != null;
        }

        /// <summary>
        /// Checks if a property should not be serialized.
        /// </summary>
        /// <param name="property">The property to check.</param>
        public static bool ShouldIgnoreProperty(this PropertyInfo property)
        {
            return property.HasAttribute<DFeIgnoreAttribute>() ||
                   !property.CanRead || !property.CanWrite;
        }

        /// <summary>
        /// Checks if a property should not be serialized.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool ShouldSerializeProperty(this PropertyInfo property, object item)
        {
            var shouldSerialize = item.GetType().GetMethod($"ShouldSerialize{property.Name}", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (shouldSerialize?.ReturnType == typeof(bool))
                return (bool)shouldSerialize.Invoke(item, null);

            return true;
        }

        public static string RemoveCData(this string value)
        {
            if (value.IsEmpty()) return value;
            return value.IsCData() ? value.GetStrBetween(9, value.Length - 4) : value;
        }

        public static bool IsCData(this string value)
        {
            if (value.IsEmpty()) return false;

            return value.StartsWith("<![CDATA[") && value.EndsWith("]]>");
        }
    }
}