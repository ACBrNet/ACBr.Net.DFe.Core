// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 05-04-2016
//
// Last Modified By : RFTD
// Last Modified On : 05-04-2016
// ***********************************************************************
// <copyright file="IDFeElement.cs" company="ACBr.Net">
//     Copyright © ACBr.Net 2014 - 2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using ACBr.Net.DFe.Core.Serializer;

namespace ACBr.Net.DFe.Core.Interfaces
{
	/// <summary>
	/// Interface IDFeElement
	/// </summary>
	public interface IDFeElement
	{
		/// <summary>
		/// Gets or sets the descricao.
		/// </summary>
		/// <value>The descricao.</value>
		string Descricao { get; set; }
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		string Id { get; set; }
		/// <summary>
		/// Gets or sets the maximum.
		/// </summary>
		/// <value>The maximum.</value>
		int Max { get; set; }
		/// <summary>
		/// Gets or sets the minimum.
		/// </summary>
		/// <value>The minimum.</value>
		int Min { get; set; }
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		string Name { get; set; }
		/// <summary>
		/// Gets or sets the ocorrencias.
		/// </summary>
		/// <value>The ocorrencias.</value>
		int Ocorrencias { get; set; }
		/// <summary>
		/// Gets or sets the tipo.
		/// </summary>
		/// <value>The tipo.</value>
		TipoCampo Tipo { get; set; }
	}
}