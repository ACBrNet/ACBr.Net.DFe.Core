using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using ACBr.Net.Core.Extensions;
using ACBr.Net.Core.Logging;
using ACBr.Net.DFe.Core.Attributes;
using ACBr.Net.DFe.Core.Extensions;
using ACBr.Net.DFe.Core.Interfaces;
using ACBr.Net.DFe.Core.Internal;

namespace ACBr.Net.DFe.Core.Serializer
{
    internal class ObjectSerializer
    {
		#region Fields

		/// <summary>
		/// The logger
		/// </summary>
		internal static IInternalLogger Logger = LoggerProvider.LoggerFor(typeof(DFeSerializer));

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
					if (Utilities.ShouldIgnoreProperty(prop) ||
					    !Utilities.ShouldSerializeProperty(prop, value))
						continue;

					var elements = Serialize(prop, value, options);
					if(elements == null)
						continue;

					foreach (var element in elements)
					{
						var child = element as XElement;
						if (child != null)
							Utilities.AddChild(child, objectElement);
						else
							Utilities.AddAttribute((XAttribute)element, objectElement);
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
		        var value = prop.GetValue(parentObject, null);
		        var objectType = ObjectType.From(prop.PropertyType);

		        if (objectType == ObjectType.Dictionary)
			        throw new NotSupportedException("Tipo Dictionary não suportado ainda !");

		        if (objectType == ObjectType.List)
			        return ListSerializer.Serialize(prop, parentObject, options);

		        if (objectType == ObjectType.DFeList)
			        return DFeListSerializer.Serialize(prop, parentObject, options);

		        if (objectType == ObjectType.InterfaceObject)
		        {
			        if (value == null)
				        return null;

			        var attributes = prop.GetAttributes<DFeItemAttribute>();
			        var attribute = attributes.SingleOrDefault(x => x.Tipo == value.GetType()) ?? attributes[0];
			        return new XObject[] { Serialize(value, value.GetType(), attribute.Name, options) };
		        }

		        if (objectType == ObjectType.ClassObject)
		        {
			        var attribute = prop.GetAttribute<DFeElementAttribute>();
			        return new XObject[] { Serialize(value, prop.PropertyType, attribute.Name, options) };
		        }

		        if (objectType == ObjectType.RootObject)
		        {
					var attribute = prop.PropertyType.GetAttribute<DFeRootAttribute>();
			        var rootElement = Serialize(value, prop.PropertyType, attribute.Name, options);
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
				var ret = Activator.CreateInstance(type);
				if (element == null)
					return ret;

				var properties = type.GetProperties();
				foreach (var prop in properties)
				{
					if (Utilities.ShouldIgnoreProperty(prop))
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
				if (objectType == ObjectType.Dictionary)
					throw new NotSupportedException("Tipo Dictionary não suportado ainda !");

				if (objectType == ObjectType.List)
				{
					var listElement = parentElement.ElementsAnyNs(tag.Name);
					return ListSerializer.Deserialize(prop.PropertyType, listElement.ToArray(), prop, options);
				}

				if (objectType == ObjectType.DFeList)
				{
					var listElement = parentElement.ElementsAnyNs(tag.Name);
					return DFeListSerializer.Deserialize(prop.PropertyType, listElement.ToArray(), prop, options);
				}

				if (objectType == ObjectType.InterfaceObject)
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

				if (objectType == ObjectType.RootObject)
				{
					var rootTag = prop.PropertyType.GetAttribute<DFeRootAttribute>();
					var xElement = parentElement.ElementsAnyNs(rootTag.Name).FirstOrDefault();
					return Deserialize(prop.PropertyType, xElement, options);
				}

				if (objectType == ObjectType.ClassObject)
				{
					var xElement = parentElement.ElementsAnyNs(tag.Name).FirstOrDefault();
					return Deserialize(prop.PropertyType, xElement, options);
				}

				var element = parentElement.ElementsAnyNs(tag.Name).FirstOrDefault() ??
				              (XObject)parentElement.Attributes(tag.Name).FirstOrDefault();

				return PrimitiveSerializer.Deserialize(tag, element, item, prop);
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