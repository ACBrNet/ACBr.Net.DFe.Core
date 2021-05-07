// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 06-05-2021
//
// Last Modified By : RFTD
// Last Modified On : 06-05-2021
// ***********************************************************************
// <copyright file="ValueElementSerializer.cs" company="ACBr.Net">
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
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using ACBr.Net.Core;
using ACBr.Net.Core.Extensions;
using ACBr.Net.DFe.Core.Attributes;
using ACBr.Net.DFe.Core.Extensions;

namespace ACBr.Net.DFe.Core.Serializer
{
    internal static class ValueElementSerializer
    {
        #region Serialize

        public static XObject[] Serialize(PropertyInfo prop, object parentObject, SerializerOptions options)
        {
            var properties = prop.PropertyType.GetProperties()
                .Where(x => !x.ShouldIgnoreProperty() && x.ShouldSerializeProperty(parentObject))
                .OrderBy(x => x.GetAttribute<DFeBaseAttribute>()?.Ordem ?? 0).ToArray();

            var valueProp = properties.Single(x => x.HasAttribute<DFeItemValueAttribute>());

            var valueType = ObjectType.From(valueProp.PropertyType);
            Guard.Against<ACBrDFeException>(valueType != ObjectType.PrimitiveType,
                $"Item {prop.Name} é do tipo [ItemValue] e o [DFeItemValueAttribute] não é do tipo primitivo");

            var value = prop.GetValue(parentObject, null);
            var attribute = prop.GetAttribute<DFeElementAttribute>();
            var attProps = properties.Where(x => x.HasAttribute<DFeAttributeAttribute>()).ToArray();

            return new[] { Serialize(attribute.Name, attribute.Namespace, value, options, valueProp, attProps) };
        }

        public static XObject Serialize(string name, string nameSpace, object parentObject, SerializerOptions options, PropertyInfo valueProp, PropertyInfo[] attProps)
        {
            XNamespace aw = nameSpace;
            var element = nameSpace.IsEmpty() ? new XElement(name) : new XElement(aw + name);

            var valueAtt = valueProp.GetAttribute<DFeItemValueAttribute>();

            var childValue = valueProp.GetValueOrIndex(parentObject);
            var estaVazio = childValue == null || childValue.ToString().IsEmpty();
            element.Value = PrimitiveSerializer.ProcessValue(ref estaVazio, valueAtt.Tipo, childValue,
                valueAtt.Ocorrencia, valueAtt.Min, valueProp, parentObject);

            foreach (var property in attProps)
            {
                var attTag = property.GetAttribute<DFeAttributeAttribute>();
                var att = (XAttribute)PrimitiveSerializer.Serialize(attTag, parentObject, property, options);
                element.AddAttribute(att);
            }

            return element;
        }

        #endregion Serialize

        #region Deserialize

        public static object Deserialize(Type type, XElement element, SerializerOptions options)
        {
            var attProps = type.GetProperties().Where(x => x.HasAttribute<DFeAttributeAttribute>()).ToArray();
            var valueProp = type.GetProperties().SingleOrDefault(x => x.HasAttribute<DFeItemValueAttribute>());

            var item = type.HasCreate() ? type.GetCreate().Invoke() : Activator.CreateInstance(type);
            if (element == null) return item;

            var valueAtt = valueProp.GetAttribute<DFeItemValueAttribute>();

            var value = PrimitiveSerializer.GetValue(valueAtt.Tipo, element.Value, item, valueProp);
            valueProp.SetValue(item, value);

            foreach (var property in attProps)
            {
                var attTag = property.GetAttribute<DFeAttributeAttribute>();
                value = PrimitiveSerializer.Deserialize(attTag, element, item, property);
                property.SetValue(item, value);
            }

            return item;
        }

        #endregion Deserialize
    }
}