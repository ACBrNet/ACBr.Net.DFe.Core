// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 05-07-2016
//
// Last Modified By : RFTD
// Last Modified On : 05-07-2016
// ***********************************************************************
// <copyright file="SignatureMethod.cs" company="ACBr.Net">
//     Copyright © ACBr.Net 2014 - 2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Xml.Serialization;
using ACBr.Net.DFe.Core.Attributes;
using ACBr.Net.DFe.Core.Serializer;

namespace ACBr.Net.DFe.Core.Document
{
	/// <summary>
	/// Class SignatureMethod.
	/// </summary>
	public class SignatureMethod
    {
		/// <summary>
		/// XS06 - Atributo Algorithm de SignatureMethod: http://www.w3.org/2000/09/xmldsig#rsa-sha1
		/// </summary>
		/// <value>The algorithm.</value>
		[DFeAttribute(TipoCampo.Str, "Algorithm", Id = "XS06", Min = 0, Max = 999, Ocorrencias = 1)]
		public string Algorithm { get; set; }
    }
}