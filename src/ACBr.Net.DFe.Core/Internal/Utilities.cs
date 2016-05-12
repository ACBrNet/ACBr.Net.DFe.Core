// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 05-03-2016
//
// Last Modified By : RFTD
// Last Modified On : 05-10-2016
// ***********************************************************************
// <copyright file="Utilities.cs" company="ACBr.Net">
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
using System.Reflection;
using System.Xml.Linq;
using ACBr.Net.DFe.Core.Attributes;
using ACBr.Net.DFe.Core.Extensions;

namespace ACBr.Net.DFe.Core.Internal
{
	internal class Utilities
	{
		#region Methods

		/// <summary>
		/// Checks if a property should not be serialized.
		/// </summary>
		/// <param name="property">The property to check.</param>
		public static bool ShouldIgnoreProperty(PropertyInfo property)
		{
			return property.HasAttribute<DFeIgnoreAttribute>() ||
			       !property.CanRead || !property.CanWrite;
		}

		/// <summary>
		/// Shoulds the ignore property.
		/// </summary>
		/// <param name="property">The property.</param>
		/// <param name="item">The item.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		public static bool ShouldSerializeProperty(PropertyInfo property, object item)
		{
			var shouldSerialize = item.GetType().GetMethod($"ShouldSerialize{property.Name}", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			if (shouldSerialize != null && shouldSerialize.ReturnType == typeof(bool))
				return (bool) shouldSerialize.Invoke(item, null);

			return true;
		}
		
		/// <summary>
		/// Gets the type of an object from its serialized XElement (e.g. if it has a type attribute) and its parent container type (e.g. if it is generic).
		/// </summary>
		/// <param name="element">The XElement representing the object used to get the type.</param>
		/// <param name="parentType">The type of the object's parent container used to check if the object is in a generic container.</param>
		/// <param name="genericArgumentIndex">The index of the object's type in the list of its parent container's generic arguments.</param>
		/// <returns>The type of an object from its serialized XElement and its parent container type.</returns>
		public static Type GetElementType(XElement element, Type parentType, int genericArgumentIndex)
		{
			Type type = null;
			var typeELement = element.Attribute("Type");
			if (typeELement != null)
				type = Type.GetType(typeELement.Value);

			if (type != null)
				return type;

			var arguments = parentType.GetGenericArguments();
			if (arguments.Length > genericArgumentIndex)
				type = arguments[genericArgumentIndex];

			return type;
		}

		/// <summary>
		/// Adds the specified XElement to the parent XElement if it is not null;
		/// </summary>
		/// <param name="child">The child XElement to add to the parent XElement.</param>
		/// <param name="parent">The parent XElement to add the child XElement to.</param>
		public static void AddChild(XElement child, XElement parent)
		{
			if (child == null || parent == null)
				return;

			parent.Add(child);
		}

		public static void AddAttribute(XAttribute child, XElement parent)
		{
			if (child == null || parent == null)
				return;

			parent.Add(child);
		}
		
		#endregion Methods
	}
}