// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 05-07-2016
//
// Last Modified By : RFTD
// Last Modified On : 05-07-2016
// ***********************************************************************
// <copyright file="X509Data.cs" company="ACBr.Net">
//     Copyright © ACBr.Net 2014 - 2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using ACBr.Net.DFe.Core.Attributes;
using ACBr.Net.DFe.Core.Serializer;

namespace ACBr.Net.DFe.Core.Document
{
	/// <summary>
	/// Class X509Data.
	/// </summary>
	public class X509Data
    {
		/// <summary>
		/// XS21 - Certificado Digital X509 em Base64
		/// </summary>
		/// <value>The X509 certificate.</value>
		[DFeElement(TipoCampo.Str, "X509Certificate", Id = "XS21", Min = 0, Max = 999, Ocorrencias = 1)]
		public string X509Certificate { get; set; }
    }
}