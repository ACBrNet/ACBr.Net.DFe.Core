// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 05-07-2016
//
// Last Modified By : RFTD
// Last Modified On : 05-07-2016
// ***********************************************************************
// <copyright file="Transform.cs" company="ACBr.Net">
//     Copyright © ACBr.Net 2014 - 2016
// </copyright>
// <summary></summary>
// ***********************************************************************

using ACBr.Net.DFe.Core.Attributes;
using ACBr.Net.DFe.Core.Serializer;

namespace ACBr.Net.DFe.Core.Document
{
	/// <summary>
	/// Class Transform.
	/// </summary>
	public class Transform
    {
		/// <summary>
		/// XS13 - Atributos válidos Algorithm do Transform:
		/// <para>http://www.w3.org/TR/2001/REC-xml-c14n-20010315</para><para>http://www.w3.org/2000/09/xmldsig#enveloped-signature</para>
		/// </summary>
		/// <value>The algorithm.</value>
		[DFeAttribute(TipoCampo.Str, "Algorithm", Id = "XS12", Min = 0, Max = 999, Ocorrencias = 1)]
		public string Algorithm { get; set; }
    }
}