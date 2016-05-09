// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 04-24-2016
//
// Last Modified By : RFTD
// Last Modified On : 04-28-2016
// ***********************************************************************
// <copyright file="DFeRootAttribute.cs" company="ACBr.Net">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace ACBr.Net.DFe.Core.Attributes
{
	/// <summary>
	/// Class DFeRootAttribute.
	/// </summary>
	/// <seealso cref="System.Attribute" />
	[AttributeUsage(AttributeTargets.Class)]
	public class DFeRootAttribute : Attribute
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="DFeRootAttribute" /> class.
		/// </summary>
		public DFeRootAttribute()
		{
			Name = string.Empty;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DFeRootAttribute" /> class.
		/// </summary>
		/// <param name="root">The Namespace.</param>
		public DFeRootAttribute(string root)
		{
			Name = root;
		}

		/// <summary>
		/// Gets or sets the Namespace.
		/// </summary>
		/// <value>The Namespace.</value>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the name space.
		/// </summary>
		/// <value>The name space.</value>
		public string Namespace { get; set; }
	}
}