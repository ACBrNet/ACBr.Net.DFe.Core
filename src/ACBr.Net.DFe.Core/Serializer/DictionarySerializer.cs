// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 06-19-2016
//
// Last Modified By : RFTD
// Last Modified On : 06-19-2016
// ***********************************************************************
// <copyright file="DictionarySerializer.cs" company="ACBr.Net">
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
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

namespace ACBr.Net.DFe.Core.Serializer
{
	internal static class DictionarySerializer
	{
		public static XObject[] Serialize(PropertyInfo prop, object parentObject, SerializerOptions options)
		{
			var element = new XElement(name);

			var dictionary = (IDictionary)value;
			foreach (DictionaryEntry dictionaryEntry in dictionary)
			{
				var keyValueElement = new XElement(elementNames);

				var keyElement = ObjectSerializer.Serialize(dictionaryEntry.Key, keyNames, null, null, null, options);
				var valueElement = ObjectSerializer.Serialize(dictionaryEntry.Value, valueNames, null, null, null, options);

				element.Add(keyValueElement);
			}

			return element;
		}

		public static object Deserialize(Type type, XElement[] parent, PropertyInfo prop, SerializerOptions options)
		{
			IDictionary dictionary;
			if (type.GetTypeInfo().IsInterface)
			{
				dictionary = new Dictionary<object, object>();
			}
			else
			{
				dictionary = (IDictionary)Activator.CreateInstance(type);
			}

			var elements = parentElement.Elements();

			foreach (var element in elements)
			{
				var keyValueElements = new List<XElement>(element.Elements());

				if (keyValueElements.Count < 2)
				{
					//No fully formed key value pair
					continue;
				}

				var keyElement = keyValueElements[0];
				var valueElement = keyValueElements[1];

				var keyType = Utilities.GetElementType(keyElement, type, 0);
				var valueType = Utilities.GetElementType(valueElement, type, 1);

				if (keyType != null && valueType != null)
				{
					var key = ObjectSerializer.Deserialize(keyType, keyElement, options);
					var value = ObjectSerializer.Deserialize(valueType, valueElement, options);

					dictionary.Add(key, value);
				}
				else
				{
					throw new InvalidOperationException("Could not deserialize this non generic dictionary without more type information.");
				}
			}

			return dictionary;
		}
	}
}