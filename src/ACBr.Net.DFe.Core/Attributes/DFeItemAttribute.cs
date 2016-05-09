// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 04-26-2016
//
// Last Modified By : RFTD
// Last Modified On : 04-26-2016
// ***********************************************************************
// <copyright file="DFeItemAttribute.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace ACBr.Net.DFe.Core.Attributes
{
	/// <summary>
	/// Class DFeItemAttribute.
	/// </summary>
	/// <seealso cref="System.Attribute" />
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
	public class DFeItemAttribute : Attribute
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="DFeElementAttribute" /> class.
		/// </summary>
		/// <param name="name">The name.</param>
		public DFeItemAttribute(string name)
		{
			Name = name;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DFeItemAttribute" /> class.
		/// </summary>
		/// <param name="tipo">The tipo.</param>
		/// <param name="name">The name.</param>
		public DFeItemAttribute(Type tipo, string name)
		{
			Tipo = tipo;
			Name = name;
		}

		#endregion Constructors

		#region Propriedades

		/// <summary>
		/// Gets or sets the tipo.
		/// </summary>
		/// <value>The tipo.</value>
		public Type Tipo { get; set; }

		/// <summary>
		/// Gets or sets the Name.
		/// </summary>
		/// <value>The Name.</value>
		public string Name { get; set; }

		#endregion Propriedades
	}
}