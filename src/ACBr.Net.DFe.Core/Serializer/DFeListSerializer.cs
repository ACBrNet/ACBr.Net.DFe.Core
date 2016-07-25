// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 05-04-2016
//
// Last Modified By : RFTD
// Last Modified On : 05-11-2016
// ***********************************************************************
// <copyright file="DFeListSerializer.cs" company="ACBr.Net">
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
using ACBr.Net.DFe.Core.Attributes;
using ACBr.Net.DFe.Core.Collection;
using ACBr.Net.DFe.Core.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace ACBr.Net.DFe.Core.Serializer
{
	internal static class DFeListSerializer
	{
		#region Serialize

		/// <summary>
		/// Serializes the specified value.
		/// </summary>
		/// <param name="prop">The property.</param>
		/// <param name="parentObject">The parent object.</param>
		/// <param name="options">The options.</param>
		/// <returns>XElement.</returns>
		public static XObject[] Serialize(PropertyInfo prop, object parentObject, SerializerOptions options)
		{
			var tag = prop.GetAttribute<DFeElementAttribute>();
			var values = ((IEnumerable<object>)prop.GetValue(parentObject, null) ?? new object[0]).ToArray();
			if (values.Length == 0 && tag.Min == 0 && tag.Ocorrencias == 0)
				return null;

			if (values.Length < tag.Min || values.Length > tag.Max)
				options.WAlerta(tag.Id, tag.Name, tag.Descricao,
					values.Length > tag.Max ? SerializerOptions.ErrMsgMaiorMaximo : SerializerOptions.ErrMsgMenorMinimo);

			if (!prop.HasAttribute<DFeItemAttribute>())
				return values.Select(value => ObjectSerializer.Serialize(value, value.GetType(), tag.Name, options)).Cast<XObject>().ToArray();

			var arrayElement = new XElement(tag.Name);
			foreach (var value in values)
			{
				var itemTags = prop.GetAttributes<DFeItemAttribute>();
				var itemTag = itemTags.SingleOrDefault(x => x.Tipo == value.GetType()) ?? itemTags[0];
				var childElement = ObjectSerializer.Serialize(value, value.GetType(), itemTag.Name, options);
				arrayElement.AddChild(childElement);
			}

			return new XObject[] { arrayElement };
		}

		#endregion Serialize

		#region Deserialize

		/// <summary>
		/// Deserializes the specified type.
		/// </summary>
		/// <param name="type">The type of the list to deserialize.</param>
		/// <param name="parent">The parent.</param>
		/// <param name="prop">The property.</param>
		/// <param name="options">Indicates how the output is deserialized.</param>
		/// <returns>The deserialized list from the XElement.</returns>
		/// <exception cref="System.InvalidOperationException">Could not deserialize this non generic dictionary without more type information.</exception>
		/// Deserializes the XElement to the list (e.g. List<T />, Array of a specified type using options.
		public static object Deserialize(Type type, XElement[] parent, PropertyInfo prop, SerializerOptions options)
		{
			var listItemType = type.IsArray ? type.GetElementType() :
				type.GetGenericArguments().Any() ? type.GetGenericArguments()[0] : type.BaseType?.GetGenericArguments()[0];
			var listType = typeof(List<>).MakeGenericType(listItemType);
			var list = (IList)Activator.CreateInstance(listType);

			if (prop.HasAttribute<DFeItemAttribute>())
			{
				var itemTags = prop.GetAttributes<DFeItemAttribute>();
				var elements = parent.Elements();
				foreach (var element in elements)
				{
					var itemTag = itemTags.SingleOrDefault(x => x.Name == element.Name) ?? itemTags[0];
					var obj = ObjectSerializer.Deserialize(itemTag.Tipo, element, options);
					list.Add(obj);
				}
			}
			else
			{
				foreach (var element in parent)
				{
					var obj = ObjectSerializer.Deserialize(listItemType, element, options);
					list.Add(obj);
				}
			}

			if (typeof(DFeCollection<>).IsAssignableFrom(type))
			{
				var dfeColType = typeof(DFeCollection<>).MakeGenericType(listItemType);
				var dfeCollection = Activator.CreateInstance(dfeColType, list);
				return dfeCollection;
			}
			else
			{
				var dfeCollection = Activator.CreateInstance(type, list);
				return dfeCollection;
			}
		}

		#endregion Deserialize
	}
}