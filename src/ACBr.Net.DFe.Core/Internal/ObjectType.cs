using System;
using System.Collections;
using ACBr.Net.DFe.Core.Attributes;
using ACBr.Net.DFe.Core.Collection;
using ACBr.Net.DFe.Core.Extensions;

namespace ACBr.Net.DFe.Core.Internal
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
		                                              || type.IsEnum
		                                              || type == typeof(DateTime);


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