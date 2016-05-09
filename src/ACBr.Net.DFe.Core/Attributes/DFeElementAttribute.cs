// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 27-03-2016
//
// Last Modified By : RFTD
// Last Modified On : 27-03-2016
// ***********************************************************************
// <copyright file="DFeElementAttribute.cs" company="ACBr.Net">
//     Copyright (c) ACBr.Net. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using ACBr.Net.DFe.Core.Interfaces;
using ACBr.Net.DFe.Core.Serializer;

namespace ACBr.Net.DFe.Core.Attributes
{
    /// <summary>
    /// Classe DFeElementAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DFeElementAttribute : Attribute, IDFeElement
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="DFeElementAttribute"/> class.
		/// </summary>
		public DFeElementAttribute()
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
		/// Initializes a new instance of the <see cref="DFeElementAttribute"/> class.
		/// </summary>
		/// <param name="tipo">The tipo.</param>
		/// <param name="name">The name.</param>
		public DFeElementAttribute(TipoCampo tipo, string name):this()
		{
			Tipo = tipo;
			Name = name;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DFeElementAttribute" /> class.
		/// </summary>
		/// <param name="tipo">The tipo.</param>
		/// <param name="id">The identifier.</param>
		/// <param name="name">The Name.</param>
		/// <param name="min">The minimum.</param>
		/// <param name="max">The maximum.</param>
		/// <param name="ocorrencias">The ocorrencias.</param>
		/// <param name="descricao">The descricao.</param>
		public DFeElementAttribute(TipoCampo tipo, string id, string name, int min,
			int max, int ocorrencias, string descricao = "") : this()
		{
			Tipo = tipo;
			Id = id;
			Name = name;
			Min = min;
			Max = max;
			Ocorrencias = ocorrencias;
			Descricao = descricao;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DFeElementAttribute"/> class.
		/// </summary>
		/// <param name="tag">The Name.</param>
		public DFeElementAttribute(string tag) : this()
		{
			Name = tag;
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
		/// Gets or sets the name space.
		/// </summary>
		/// <value>The name space.</value>
		public string Namespace { get; set; }

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