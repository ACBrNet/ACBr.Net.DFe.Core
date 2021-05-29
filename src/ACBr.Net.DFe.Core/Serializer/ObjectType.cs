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
using System;
using System.Collections;
using System.Linq;
using ACBr.Net.Core;
using ACBr.Net.Core.Extensions;

namespace ACBr.Net.DFe.Core.Serializer
{
    internal readonly struct ObjectType
    {
        #region ObjectsTypes

        public static ObjectType ClassType => new ObjectType(0);

        public static ObjectType PrimitiveType => new ObjectType(1);

        public static ObjectType ListType => new ObjectType(2);

        public static ObjectType DictionaryType => new ObjectType(3);

        public static ObjectType InterfaceType => new ObjectType(4);

        public static ObjectType RootType => new ObjectType(5);

        public static ObjectType ArrayType => new ObjectType(6);

        public static ObjectType EnumerableType => new ObjectType(7);

        public static ObjectType AbstractType => new ObjectType(8);

        public static ObjectType ValueElementType => new ObjectType(9);

        #endregion ObjectsTypes

        #region Constructors

        private ObjectType(int id)
        {
            Guard.Against<ArgumentException>(!id.IsBetween(0, 9), "Tipo de objeto desconhecido.");

            Id = id;
        }

        #endregion Constructors

        #region Propriedades

        private int Id { get; }

        #endregion Propriedades

        #region Operators

        public static bool operator ==(ObjectType a, ObjectType b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(ObjectType a, ObjectType b)
        {
            return !(a == b);
        }

        #endregion Operators

        #region Methods

        public static ObjectType From(object value)
        {
            return From(value.GetType());
        }

        public static ObjectType From(Type type)
        {
            if (IsPrimitive(type)) return PrimitiveType;
            if (IsInterface(type)) return InterfaceType;
            if (IsAbstract(type)) return AbstractType;
            if (IsList(type)) return ListType;
            if (IsDictionary(type)) return DictionaryType;
            if (IsArray(type)) return ArrayType;
            if (IsEnumerable(type)) return EnumerableType;
            if (IsValueElement(type)) return ValueElementType;

            return IsRoot(type) ? RootType : ClassType;
        }

        public override bool Equals(object obj)
        {
            return obj != null && GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        private static bool IsPrimitive(Type type)
        {
            return type == typeof(string)
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
                   || type == typeof(DateTimeOffset)
                   || type.IsEnum
                   || type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && IsPrimitive(type.GetGenericArguments()[0]);
        }

        private static bool IsList(Type type)
        {
            return typeof(IList).IsAssignableFrom(type) && !IsArray(type);
        }

        private static bool IsEnumerable(Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type) && !IsArray(type);
        }

        private static bool IsInterface(Type type)
        {
            return type.IsInterface && !IsList(type) && !IsEnumerable(type) && !IsArray(type);
        }

        private static bool IsAbstract(Type type)
        {
            return type.IsAbstract && !IsList(type) && !IsEnumerable(type) && !IsArray(type);
        }

        private static bool IsArray(Type type)
        {
            return type.IsArray;
        }

        private static bool IsRoot(Type type)
        {
            return type.HasAttribute<DFeRootAttribute>();
        }

        public static bool IsDictionary(Type type)
        {
            return typeof(IDictionary).IsAssignableFrom(type);
        }

        private static bool IsValueElement(Type type)
        {
            return type.IsClass && !IsPrimitive(type) &&
                   type.GetProperties().Count(x => x.HasAttribute<DFeItemValueAttribute>()) == 1 &&
                   type.GetProperties().All(x => x.HasAttribute<DFeItemValueAttribute>() ||
                                                 x.HasAttribute<DFeAttributeAttribute>() ||
                                                 x.HasAttribute<DFeIgnoreAttribute>());
        }

        #endregion Methods
    }
}