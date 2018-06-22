// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 05-04-2018
//
// Last Modified By : RFTD
// Last Modified On : 05-04-2018
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
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using ACBr.Net.Core.Exceptions;
using ACBr.Net.Core.Extensions;
using ACBr.Net.Core.Logging;
using ACBr.Net.DFe.Core.Attributes;

namespace ACBr.Net.DFe.Core.Serializer
{
    internal static class DictionarySerializer
    {
        #region Fields

        /// <summary>
        /// The logger
        /// </summary>
        internal static IACBrLogger Logger = LoggerProvider.LoggerFor<DFeSerializer>();

        #endregion Fields

        #region Serialize

        public static XObject[] Serialize(PropertyInfo prop, object parentObject, SerializerOptions options)
        {
            var tag = prop.GetAttribute<DFeDictionaryAttribute>();

            var dictionary = (IDictionary)prop.GetValue(parentObject, null);
            if (dictionary.Count < tag.MinSize || dictionary.Count > tag.MaxSize && tag.MaxSize > 0)
            {
                var msg = dictionary.Count > tag.MaxSize ? DFeSerializer.ErrMsgMaiorMaximo : DFeSerializer.ErrMsgMenorMinimo;
                options.AddAlerta(tag.Id, tag.Name, tag.Descricao, msg);
            }

            if (dictionary.Count == 0 && tag.MinSize == 0 && tag.Ocorrencia == Ocorrencia.NaoObrigatoria) return null;

            var keyAtt = prop.GetAttribute<DFeDictionaryKeyAttribute>();
            var valueAtt = prop.GetAttribute<DFeDictionaryValueAttribute>();

            var args = dictionary.GetType().BaseType?.GetGenericArguments() ?? dictionary.GetType().GetGenericArguments();

            Guard.Against<ArgumentException>(args.Length != 2);

            var keyType = ObjectType.From(args[0]);
            var valueType = ObjectType.From(args[1]);

            Guard.Against<ACBrDFeException>(keyType != ObjectType.PrimitiveType && keyAtt.AsAttribute);

            var elementName = tag.ItemName.IsEmpty() ? tag.Name : tag.ItemName;
            var list = new List<XElement>();

            for (var i = 0; i < dictionary.Count; i++)
            {
                var entry = (DictionaryEntry)dictionary[i]; ;
                var key = entry.Key;
                var value = entry.Value;

                var keyElement = keyType == ObjectType.PrimitiveType
                    ? PrimitiveSerializer.SerializeDictionaryKey(keyAtt, dictionary, prop, options, i)
                    : ObjectSerializer.Serialize(key, key.GetType(), keyAtt.Name, keyAtt.Namespace, options);

                var valueElement = valueType == ObjectType.PrimitiveType
                    ? PrimitiveSerializer.SerializeDictionaryKey(valueAtt, dictionary, prop, options, i)
                    : ObjectSerializer.Serialize(value, value.GetType(), valueAtt.Name, valueAtt.Namespace, options);

                var dicElement = new XElement(elementName);

                if (keyAtt.AsAttribute)
                {
                    ((XElement)valueElement).AddAttribute((XAttribute)keyElement);
                    dicElement.Add((XElement)valueElement);
                }
                else
                {
                    dicElement.AddChild((XElement)keyElement);
                    dicElement.AddChild((XElement)valueElement);
                }

                list.Add(dicElement);
            }

            if (tag.ItemName.IsEmpty())
            {
                return list.ToArray();
            }

            var element = new XElement(tag.Name, tag.Namespace);
            element.AddChild(list.ToArray());

            return new XObject[] { element };
        }

        #endregion Serialize

        #region Deserialize

        public static object Deserialize(Type type, XElement[] parent, PropertyInfo prop, object parentItem, SerializerOptions options)
        {
            return null;
        }

        #endregion Deserialize
    }
}