// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 05-07-2016
//
// Last Modified By : RFTD
// Last Modified On : 05-07-2016
// ***********************************************************************
// <copyright file="XDocumentExtensions.cs" company="ACBr.Net">
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
using System.Reflection;
using System.Xml.Linq;
using ACBr.Net.Core.Extensions;
using ACBr.Net.DFe.Core.Attributes;
using ACBr.Net.DFe.Core.Service;

namespace ACBr.Net.DFe.Core.Extensions
{
    internal static class XDocumentExtensions
    {
        public static Type GetElementType(this XElement element, Type parentType, int genericArgumentIndex)
        {
            Type type = null;
            var typeELement = element.Attribute("Type");
            if (typeELement != null)
            {
                type = Type.GetType(typeELement.Value);
            }

            if (type != null) return type;

            var arguments = parentType.GetGenericArguments();
            if (arguments.Length > genericArgumentIndex)
            {
                type = arguments[genericArgumentIndex];
            }

            return type;
        }

        public static XElement[] GetElements(this XElement element, PropertyInfo prop)
        {
            var listElement = new List<XElement>();

            var tag = prop.GetAttribute<DFeBaseAttribute>();

            var itemElement = element.ElementsAnyNs(tag.Name);
            if (!itemElement.IsNullOrEmpty())
                listElement.AddRange(itemElement);

            foreach (var att in prop.GetAttributes<DFeItemAttribute>())
            {
                itemElement = element.ElementsAnyNs(att.Name);
                if (!itemElement.IsNullOrEmpty())
                    listElement.AddRange(itemElement);
            }

            return listElement.ToArray();
        }

        public static void AddChilds(this XElement element, params XObject[] childs)
        {
            foreach (var child in childs)
            {
                if (child is XElement childElement)
                    element.AddChild(childElement);
                else
                    element.AddAttribute((XAttribute)child);
            }
        }
    }
}