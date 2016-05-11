// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 04-24-2016
//
// Last Modified By : RFTD
// Last Modified On : 04-24-2016
// ***********************************************************************
// <copyright file="DFeAttributeAttribute.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using ACBr.Net.DFe.Core.Interfaces;
using ACBr.Net.DFe.Core.Serializer;

namespace ACBr.Net.DFe.Core.Attributes
{
	/// <summary>
	/// Class DFeAttributeAttribute.
	/// </summary>
	/// <seealso cref="System.Attribute" />
	[AttributeUsage(AttributeTargets.Property)]
	public class DFeAttributeAttribute : Attribute, IDFeElement
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="DFeElementAttribute" /> class.
		/// </summary>
		public DFeAttributeAttribute()
		{
			Tipo = TipoCampo.Str;
			Id = "";
			Name = string.Empty;
			Min = 0;
			Max = 0;
			Ocorrencias = 0;
			Descricao = string.Empty;
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="DFeElementAttribute" /> class.
		/// </summary>
		/// <param name="name">The Name.</param>
		public DFeAttributeAttribute(string name) : this()
		{
			Name = name;
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="DFeElementAttribute" /> class.
		/// </summary>
		/// <param name="tipo">The tipo.</param>
		/// <param name="name">The name.</param>
		public DFeAttributeAttribute(TipoCampo tipo, string name) : this()
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
		public TipoCampo Tipo { get; set; }

		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the Name.
		/// </summary>
		/// <value>The Name.</value>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the minimum.
		/// </summary>
		/// <value>The minimum.</value>
		public int Min { get; set; }

		/// <summary>
		/// Gets or sets the maximum.
		/// </summary>
		/// <value>The maximum.</value>
		public int Max { get; set; }

		/// <summary>
		/// Gets or sets the ocorrencias.
		/// </summary>
		/// <value>The ocorrencias.</value>
		public int Ocorrencias { get; set; }

		/// <summary>
		/// Gets or sets the descricao.
		/// </summary>
		/// <value>The descricao.</value>
		public string Descricao { get; set; }

		#endregion Propriedades
	}
}