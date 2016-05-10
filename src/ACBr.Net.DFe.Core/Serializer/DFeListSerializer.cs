using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using ACBr.Net.DFe.Core.Attributes;
using ACBr.Net.DFe.Core.Collection;
using ACBr.Net.DFe.Core.Extensions;
using ACBr.Net.DFe.Core.Internal;

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
			var values = ((IEnumerable<object>) prop.GetValue(parentObject, null) ?? new object[0]).ToArray();
			if (values.Length == 0 && tag.Min == 0 && tag.Ocorrencias == 0)
				return null;

			if (values.Length < tag.Min || values.Length > tag.Max)
				options.WAlerta(tag.Id, tag.Name, tag.Descricao,
					values.Length > tag.Max ? SerializerOptions.ErrMsgMaiorMaximo : SerializerOptions.ErrMsgMenorMinimo);


			if (!prop.HasAttribute<DFeItemAttribute>())
				return values.Select(value => ObjectSerializer.Serialize(value, tag.Name, options)).Cast<XObject>().ToArray();

			var arrayElement = new XElement(tag.Name);
			foreach (var value in values)
			{
				var itemTags = prop.GetAttributes<DFeItemAttribute>();
				var itemTag = itemTags.SingleOrDefault(x => x.Tipo == value.GetType()) ?? itemTags[0];
				var childElement = ObjectSerializer.Serialize(value, itemTag.Name, options);
				Utilities.AddChild(childElement, arrayElement);
			}

			return new XObject[] {arrayElement};
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