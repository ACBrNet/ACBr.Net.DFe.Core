// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 05-04-2016
//
// Last Modified By : RFTD
// Last Modified On : 05-11-2016
// ***********************************************************************
// <copyright file="ObjectSerializer.cs" company="ACBr.Net">
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

using ACBr.Net.Core.Extensions;
using ACBr.Net.Core.Logging;
using ACBr.Net.DFe.Core.Attributes;
using ACBr.Net.DFe.Core.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace ACBr.Net.DFe.Core.Serializer
{
	internal class ObjectSerializer
	{
		#region Fields

		/// <summary>
		/// The logger
		/// </summary>
		internal static IACBrLogger Logger = LoggerProvider.LoggerFor<DFeSerializer>();

		#endregion Fields

		#region Serialize

		/// <summary>
		/// Serializes the specified object to a XElement using options.
		/// </summary>
		/// <param name="value">The object to serialize.</param>
		/// <param name="tipo">The tipo.</param>
		/// <param name="name">The name of the object to serialize.</param>
		/// <param name="options">Indicates how the output is formatted or serialized.</param>
		/// <returns>The XElement representation of the object.</returns>
		/// <exception cref="ACBrDFeException"></exception>
		public static XElement Serialize(object value, Type tipo, string name, SerializerOptions options)
		{
			try
			{
				XNamespace aw = "";
				if (tipo.HasAttribute<DFeRootAttribute>())
				{
					var attribute = tipo.GetAttribute<DFeRootAttribute>();
					if (!attribute.Namespace.IsEmpty())
						aw = attribute.Namespace;
				}
				else if (tipo.HasAttribute<DFeElementAttribute>())
				{
					var attribute = tipo.GetAttribute<DFeElementAttribute>();
					if (!attribute.Namespace.IsEmpty())
						aw = attribute.Namespace;
				}

				var objectElement = new XElement(aw + name);
				var properties = tipo.GetProperties();
				foreach (var prop in properties)
				{
					if (prop.ShouldIgnoreProperty() || !prop.ShouldSerializeProperty(value)) continue;

					var elements = Serialize(prop, value, options);
					if (elements == null) continue;

					foreach (var element in elements)
					{
						var child = element as XElement;
						if (child != null)
							objectElement.AddChild(child);
						else
							objectElement.AddAttribute((XAttribute)element);
					}
				}

				return objectElement;
			}
			catch (Exception e)
			{
				var msg = $"Erro ao serializar o objeto:{Environment.NewLine}{value}";
				Logger.Error(msg, e);
				throw new ACBrDFeException(msg, e);
			}
		}

		/// <summary>
		/// Serializes the specified property into a XElement using options.
		/// </summary>
		/// <param name="prop">The property to serialize.</param>
		/// <param name="parentObject">The object that owns the property.</param>
		/// <param name="options">Indicates how the output is formatted or serialized.</param>
		/// <returns>The XElement representation of the property. May be null if it has no value, cannot be read or written or should be ignored.</returns>
		private static IEnumerable<XObject> Serialize(PropertyInfo prop, object parentObject, SerializerOptions options)
		{
			try
			{
				var objectType = ObjectType.From(prop.PropertyType);

				if (objectType == ObjectType.DictionaryType) throw new NotSupportedException("Tipo Dictionary não suportado ainda !");

				if (objectType.IsIn(ObjectType.ListType, ObjectType.ArrayType, ObjectType.EnumerableType))
				{
					return ListSerializer.Serialize(prop, parentObject, options);
				}

				var value = prop.GetValue(parentObject, null);

				if (objectType == ObjectType.InterfaceType)
				{
					if (value == null) return null;

					var attributes = prop.GetAttributes<DFeItemAttribute>();
					var attribute = attributes.SingleOrDefault(x => x.Tipo == value.GetType()) ?? attributes[0];
					return new XObject[] { Serialize(value, value.GetType(), attribute.Name, options) };
				}

				if (objectType == ObjectType.ClassType)
				{
					var attribute = prop.GetAttribute<DFeElementAttribute>();
					return new XObject[] { Serialize(value, prop.PropertyType, attribute.Name, options) };
				}

				if (objectType == ObjectType.RootType)
				{
					var rooTag = prop.PropertyType.GetAttribute<DFeRootAttribute>();
					var rootName = rooTag.Name;

					if (rootName.IsEmpty())
					{
						var root = prop.PropertyType.GetRootName(value);
						rootName = root.IsEmpty() ? prop.PropertyType.Name : root;
					}

					var rootElement = Serialize(value, prop.PropertyType, rootName, options);
					return new XObject[] { rootElement };
				}

				var tag = prop.GetTag();
				return new[] { PrimitiveSerializer.Serialize(tag, parentObject, prop, options) };
			}
			catch (Exception e)
			{
				var msg = $"Erro ao serializar a propriedade:{Environment.NewLine}{prop.DeclaringType?.Name ?? prop.PropertyType.Name} - {prop.Name}";
				Logger.Error(msg, e);
				throw new ACBrDFeException(msg, e);
			}
		}

		#endregion Serialize

		#region Deserialize

		/// <summary>
		/// Deserializes the XElement to the specified .NET type using options.
		/// </summary>
		/// <param name="type">The type of the deserialized .NET object.</param>
		/// <param name="element">The XElement to deserialize.</param>
		/// <param name="options">Indicates how the output is deserialized.</param>
		/// <returns>The deserialized object from the XElement.</returns>
		public static object Deserialize(Type type, XElement element, SerializerOptions options)
		{
			try
			{
				var ret = type.HasCreate() ? type.GetCreate().Invoke() :
											 Activator.CreateInstance(type);

				if (element == null)
					return ret;

				var properties = type.GetProperties();
				foreach (var prop in properties)
				{
					if (prop.ShouldIgnoreProperty())
						continue;

					var value = Deserialize(prop, element, ret, options);
					prop.SetValue(ret, value, null);
				}

				return ret;
			}
			catch (Exception e)
			{
				var msg = $"Erro ao deserializar o objeto:{Environment.NewLine}{type.Name} - {element.Name}";
				Logger.Error(msg, e);
				throw new ACBrDFeException(msg, e);
			}
		}

		/// <summary>
		/// Deserializes the XElement to the object of a specified type using options.
		/// </summary>
		/// <param name="prop">The property.</param>
		/// <param name="parentElement">The parent XElement used to deserialize the object.</param>
		/// <param name="item">The item.</param>
		/// <param name="options">Indicates how the output is deserialized.</param>
		/// <returns>The deserialized object from the XElement.</returns>
		/// <exception cref="System.NotSupportedException">Tipo Dictionary não suportado ainda !</exception>
		public static object Deserialize(PropertyInfo prop, XElement parentElement, object item, SerializerOptions options)
		{
			try
			{
				var tag = prop.HasAttribute<DFeElementAttribute>()
					? (IDFeElement)prop.GetAttribute<DFeElementAttribute>()
					: prop.GetAttribute<DFeAttributeAttribute>();

				var objectType = ObjectType.From(prop.PropertyType);
				if (objectType == ObjectType.DictionaryType) throw new NotSupportedException("Tipo Dictionary não suportado ainda !");

				if (objectType.IsIn(ObjectType.ArrayType, ObjectType.EnumerableType))
				{
					var listElement = parentElement.ElementsAnyNs(tag.Name);
					var list = (ArrayList)ListSerializer.Deserialize(typeof(ArrayList), listElement.ToArray(), prop, item, options);
					var type = prop.PropertyType.IsArray ? prop.PropertyType.GetElementType() : prop.PropertyType.GetGenericArguments()[0];
					return objectType == ObjectType.ArrayType ? list.ToArray(type) : list.Cast(type);
				}

				if (objectType == ObjectType.ListType)
				{
					var listElement = parentElement.ElementsAnyNs(tag.Name);
					return ListSerializer.Deserialize(prop.PropertyType, listElement.ToArray(), prop, item, options);
				}

				if (objectType == ObjectType.InterfaceType)
				{
					var tags = prop.GetAttributes<DFeItemAttribute>();
					foreach (var att in tags)
					{
						var node = parentElement.ElementsAnyNs(att.Name).FirstOrDefault();
						if (node == null)
							continue;

						return Deserialize(att.Tipo, node, options);
					}
				}

				if (objectType == ObjectType.RootType)
				{
					var rootTag = prop.PropertyType.GetAttribute<DFeRootAttribute>();
					var rootNames = new List<string>();
					if (!rootTag.Name.IsEmpty())
					{
						rootNames.Add(rootTag.Name);
						rootNames.Add(prop.PropertyType.Name);
					}
					else
					{
						rootNames.AddRange(prop.PropertyType.GetRootNames());
						rootNames.Add(prop.PropertyType.Name);
					}

					var xmlNode = (from node in parentElement.Elements()
								   where node.Name.LocalName.IsIn(rootNames)
								   select node).FirstOrDefault();

					return Deserialize(prop.PropertyType, xmlNode, options);
				}

				if (objectType == ObjectType.ClassType)
				{
					var xElement = parentElement.ElementsAnyNs(tag.Name).FirstOrDefault();
					return Deserialize(prop.PropertyType, xElement, options);
				}

				var element = parentElement.ElementsAnyNs(tag.Name).FirstOrDefault() ??
							  (XObject)parentElement.Attributes(tag.Name).FirstOrDefault();

				return PrimitiveSerializer.Deserialize(tag, element, item, prop, options);
			}
			catch (Exception e)
			{
				var msg = $"Erro ao deserializar a propriedade:{Environment.NewLine}{prop.DeclaringType?.Name ?? prop.PropertyType.Name} - {prop.Name}";
				Logger.Error(msg, e);
				throw new ACBrDFeException(msg, e);
			}
		}

		#endregion Deserialize
	}
}