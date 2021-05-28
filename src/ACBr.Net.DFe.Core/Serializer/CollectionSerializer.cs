// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 05-04-2016
//
// Last Modified By : RFTD
// Last Modified On : 05-11-2016
// ***********************************************************************
// <copyright file="CollectionSerializer.cs" company="ACBr.Net">
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
using ACBr.Net.DFe.Core.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using ACBr.Net.Core;
using ACBr.Net.DFe.Core.Extensions;

namespace ACBr.Net.DFe.Core.Serializer
{
    internal static class CollectionSerializer
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
            var tag = prop.GetAttribute<DFeCollectionAttribute>();
            var list = (ICollection)prop.GetValue(parentObject, null) ?? new ArrayList();

            if (list.Count < tag.MinSize || list.Count > tag.MaxSize && tag.MaxSize > 0)
            {
                var msg = list.Count > tag.MaxSize ? DFeSerializer.ErrMsgMaiorMaximo : DFeSerializer.ErrMsgMenorMinimo;
                options.AddAlerta(tag.Id, tag.Name, tag.Descricao, msg);
            }

            if (list.Count == 0 && tag.MinSize == 0 && tag.Ocorrencia == Ocorrencia.NaoObrigatoria) return null;

            XElement arrayElement = null;
            if (!tag.Name.IsEmpty() && prop.HasAttribute<DFeItemAttribute>())
                arrayElement = new XElement(tag.Name);

            var itemType = GetItemType(prop.PropertyType) ?? GetItemType(list.GetType());
            var objectType = ObjectType.From(itemType);

            XElement[] childs;
            if (objectType == ObjectType.PrimitiveType)
                childs = SerializePrimitive(prop, parentObject, list, tag, options);
            else if (objectType == ObjectType.ValueElementType)
                childs = SerializeElementValue(list, tag, prop.GetAttributes<DFeItemAttribute>(), options);
            else
                childs = !prop.HasAttribute<DFeItemAttribute>() ? SerializeObjects(list, tag, options) :
                                                                  SerializeChild(list, tag, prop.GetAttributes<DFeItemAttribute>(), options);

            arrayElement?.AddChild(childs.ToArray());

            return arrayElement != null ? childs.Cast<XObject>().ToArray() : new XObject[] { arrayElement };
        }

        public static XElement[] SerializeChild(ICollection values, DFeCollectionAttribute tag, DFeItemAttribute[] itemTags, SerializerOptions options)
        {
            var childElements = new List<XElement>();

            foreach (var value in values)
            {
                var itemTag = itemTags.SingleOrDefault(x => x.Tipo == value.GetType());
                Guard.Against<ACBrDFeException>(itemTag == null, $"Item {value.GetType().Name} não presente na lista de itens.");

                var childElement = ObjectSerializer.Serialize(value, value.GetType(), itemTag.Name, itemTag.Namespace, options);
                childElements.Add(childElement);
            }

            return childElements.ToArray();
        }

        public static XElement[] SerializeObjects(ICollection values, DFeCollectionAttribute tag, SerializerOptions options)
        {
            return (from object value in values select ObjectSerializer.Serialize(value, value.GetType(), tag.Name, tag.Namespace, options)).ToArray();
        }

        public static XElement[] SerializePrimitive(PropertyInfo prop, object parentObject, ICollection values, DFeCollectionAttribute tag, SerializerOptions options)
        {
            var retElements = new List<XElement>();
            for (var i = 0; i < values.Count; i++)
            {
                var ret = (XElement)PrimitiveSerializer.Serialize(tag, parentObject, prop, options, i);
                retElements.Add(ret);
            }

            return retElements.ToArray();
        }

        public static XElement[] SerializeElementValue(ICollection values, DFeCollectionAttribute tag, DFeItemAttribute[] itemTags, SerializerOptions options)
        {
            var retElements = new List<XElement>();
            foreach (var value in values)
            {
                var itemTag = itemTags.SingleOrDefault(x => x.Tipo == value.GetType());
                Guard.Against<ACBrDFeException>(itemTag == null, $"Item {value.GetType().Name} não presente na lista de itens.");

                var properties = value.GetType().GetProperties()
                    .Where(x => !x.ShouldIgnoreProperty() && x.ShouldSerializeProperty(value))
                    .OrderBy(x => x.GetAttribute<DFeBaseAttribute>()?.Ordem ?? 0).ToArray();

                var valueProp = properties.Single(x => x.HasAttribute<DFeItemValueAttribute>());

                var valueType = ObjectType.From(valueProp.PropertyType);
                Guard.Against<ACBrDFeException>(valueType != ObjectType.PrimitiveType,
                    $"Item {value.GetType().Name} é do tipo [ItemValue] e o [DFeItemValueAttribute] não é do tipo primitivo");

                var attProps = properties.Where(x => x.HasAttribute<DFeAttributeAttribute>()).ToArray();

                var element = ValueElementSerializer.Serialize(itemTag.Name, itemTag.Namespace, value, options, valueProp, attProps);
                retElements.Add(element);
            }

            return retElements.ToArray();
        }

        #endregion Serialize

        #region Deserialize

        /// <summary>
        /// Deserializes the specified type.
        /// </summary>
        /// <param name="type">The type of the list to deserialize.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="prop">The property.</param>
        /// <param name="parentItem"></param>
        /// <param name="options">Indicates how the output is deserialized.</param>
        /// <returns>The deserialized list from the XElement.</returns>
        /// <exception cref="System.InvalidOperationException">Could not deserialize this non generic dictionary without more type information.</exception>
        /// Deserializes the XElement to the list (e.g. List<T />, Array of a specified type using options.
        public static object Deserialize(Type type, XElement[] parent, PropertyInfo prop, object parentItem, SerializerOptions options)
        {
            var listItemType = GetListType(type);
            var objectType = ObjectType.From(GetItemType(prop.PropertyType));

            var list = (IList)Activator.CreateInstance(type);
            var elementAtt = prop.GetAttribute<DFeCollectionAttribute>();

            if (prop.HasAttribute<DFeItemAttribute>())
            {
                var itemTags = prop.GetAttributes<DFeItemAttribute>();
                var elements = parent.All(x => x.Name.LocalName == elementAtt.Name) && parent.Length > 1 ? parent : parent.Elements();

                foreach (var element in elements)
                {
                    var itemTag = itemTags.SingleOrDefault(x => x.Name == element.Name.LocalName);
                    Guard.Against<ACBrDFeException>(itemTag == null, $"Nenhum atributo [{nameof(DFeItemAttribute)}] encontrado " +
                                                                     $"para o elemento: {element.Name.LocalName}");

                    object item;
                    if (objectType == ObjectType.ValueElementType)
                        item = ValueElementSerializer.Deserialize(itemTag.Tipo, element, options);
                    else if (objectType == ObjectType.PrimitiveType)
                        item = PrimitiveSerializer.Deserialize(elementAtt, element, parentItem, prop);
                    else
                        item = ObjectSerializer.Deserialize(itemTag.Tipo, element, options);

                    list.Add(item);
                }
            }
            else
            {
                if (objectType == ObjectType.PrimitiveType)
                {
                    foreach (var element in parent)
                    {
                        var obj = PrimitiveSerializer.Deserialize(elementAtt, element, parentItem, prop);
                        list.Add(obj);
                    }
                }
                else
                {
                    if (ObjectType.From(prop.PropertyType).IsIn(ObjectType.ArrayType, ObjectType.EnumerableType))
                        listItemType = GetItemType(prop.PropertyType);

                    foreach (var element in parent)
                    {
                        var item = objectType == ObjectType.ValueElementType ? ValueElementSerializer.Deserialize(listItemType, element, options) :
                                                                               ObjectSerializer.Deserialize(listItemType, element, options);
                        list.Add(item);
                    }
                }
            }

            return list;
        }

        public static object Deserialize(Type type, XElement[] elements, SerializerOptions options)
        {
            var listItemType = GetListType(type);
            var list = (IList)Activator.CreateInstance(type);

            foreach (var element in elements)
            {
                var obj = ObjectSerializer.Deserialize(listItemType, element, options);
                list.Add(obj);
            }

            return list;
        }

        #endregion Deserialize

        #region Methods

        private static Type GetListType(Type type)
        {
            var listItemType = typeof(ArrayList).IsAssignableFrom(type) || type.IsArray ? typeof(ArrayList) :
                               type.GetGenericArguments().Any() ? type.GetGenericArguments()[0] : type.BaseType?.GetGenericArguments()[0];

            return listItemType;
        }

        public static Type GetItemType(Type type)
        {
            var listItemType = type.IsArray ? type.GetElementType() :
                               type.GetGenericArguments().Any() ? type.GetGenericArguments()[0] : type.BaseType?.GetGenericArguments()[0];

            return listItemType;
        }

        #endregion Methods
    }
}