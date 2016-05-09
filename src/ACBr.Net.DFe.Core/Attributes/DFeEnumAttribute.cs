// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 05-08-2016
//
// Last Modified By : RFTD
// Last Modified On : 05-08-2016
// ***********************************************************************
// <copyright file="DFeEnumAttribute.cs" company="ACBr.Net">
//     Copyright © ACBr.Net 2014 - 2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace ACBr.Net.DFe.Core.Attributes
{
	/// <summary>
	/// Class DFeEnumAttribute. This class cannot be inherited.
	/// </summary>
	/// <seealso cref="System.Attribute" />
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class DFeEnumAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DFeEnumAttribute"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		public DFeEnumAttribute(string value)
		{
			Value = value;
		}

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>The value.</value>
		public string Value { get; set; }
	}
}