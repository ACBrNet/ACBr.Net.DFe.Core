// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 05-03-2016
//
// Last Modified By : RFTD
// Last Modified On : 05-11-2016
// ***********************************************************************
// <copyright file="ObjectType.cs" company="ACBr.Net">
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

namespace ACBr.Net.DFe.Core.Serializer
{
	internal struct ObjectType
	{
		public static ObjectType Primitive => new ObjectType(1);
		public static ObjectType List => new ObjectType(2);
		public static ObjectType Dictionary => new ObjectType(3);
		public static ObjectType DFeList => new ObjectType(4);
		public static ObjectType InterfaceObject => new ObjectType(5);
		public static ObjectType RootObject => new ObjectType(6);
		public static ObjectType ClassObject => new ObjectType(10);

		private ObjectType(int id)
		{
			Id = id;
		}

		public static ObjectType From(object value)
		{
			return From(value.GetType());
		}

		public static ObjectType From(Type type)
		{
			if (IsPrimitive(type))
				return Primitive;
			if (IsDictionary(type))
				return Dictionary;
			if (IsList(type))
				return List;
			if (IsInterfaceObject(type))
				return InterfaceObject;
			if (IsRootObject(type))
				return RootObject;

			return IsDFeList(type) ? DFeList : ClassObject;
		}

		private int Id { get; }

		public override bool Equals(object obj)
		{
			return GetHashCode() == obj.GetHashCode();
		}

		public static bool operator ==(ObjectType a, ObjectType b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(ObjectType a, ObjectType b)
		{
			return !(a == b);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}

		/// <summary>
		/// Checks if the type is a fundamental primitive object (e.g string, int etc.).
		/// </summary>
		/// <param name="type">The type to check.</param>
		/// <returns>The boolean value indicating whether the type is a fundamental primitive.</returns>
		private static bool IsPrimitive(Type type) => type == typeof(string)
													  || type == typeof(char)
													  || type == typeof(sbyte)
													  || type == typeof(short)
													  || type == typeof(int)
													  || type == typeof(long)
													  || type == typeof(byte)
													  || type == typeof(ushort)
													  || type == typeof(uint)
													  || type == typeof(ulong)
													  || type == typeof(double)
													  || type == typeof(float)
													  || type == typeof(decimal)
													  || type == typeof(bool)
													  || type == typeof(DateTime)
													  || type == typeof(char?)
													  || type == typeof(sbyte?)
													  || type == typeof(short?)
													  || type == typeof(int?)
													  || type == typeof(long?)
													  || type == typeof(byte?)
													  || type == typeof(ushort?)
													  || type == typeof(uint?)
													  || type == typeof(ulong?)
													  || type == typeof(double?)
													  || type == typeof(float?)
													  || type == typeof(decimal?)
													  || type == typeof(bool?)
													  || type == typeof(DateTime?)
													  || type.IsEnum;

		/// <summary>
		/// Determines whether the specified type is list.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns><c>true</c> if the specified type is list; otherwise, <c>false</c>.</returns>
		private static bool IsList(Type type)
		{
			return typeof(ICollection).IsAssignableFrom(type) ||
				(type.BaseType != null && typeof(ICollection).IsAssignableFrom(type.BaseType));
		}

		/// <summary>
		/// Determines whether [is d fe list] [the specified type].
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns><c>true</c> if [is d fe list] [the specified type]; otherwise, <c>false</c>.</returns>
		private static bool IsDFeList(Type type)
		{
			return (type.IsGenericType && typeof(DFeCollection<>).IsAssignableFrom(type.GetGenericTypeDefinition())) ||
					(type.BaseType != null && type.BaseType.IsGenericType && typeof(DFeCollection<>).IsAssignableFrom(type.BaseType.GetGenericTypeDefinition()));
		}

		private static bool IsInterfaceObject(Type type)
		{
			return type.IsInterface;
		}

		private static bool IsRootObject(Type type)
		{
			return type.HasAttribute<DFeRootAttribute>();
		}

		/// <summary>
		/// Determines whether the specified type is dictionary.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns><c>true</c> if the specified type is dictionary; otherwise, <c>false</c>.</returns>
		public static bool IsDictionary(Type type)
		{
			return typeof(IDictionary).IsAssignableFrom(type);
		}
	}
}