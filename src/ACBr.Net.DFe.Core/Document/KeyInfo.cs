// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 05-07-2016
//
// Last Modified By : RFTD
// Last Modified On : 05-07-2016
// ***********************************************************************
// <copyright file="KeyInfo.cs" company="ACBr.Net">
//     Copyright © ACBr.Net 2014 - 2016
// </copyright>
// <summary></summary>
// ***********************************************************************

using ACBr.Net.DFe.Core.Attributes;

namespace ACBr.Net.DFe.Core.Document
{
	/// <summary>
	/// Class KeyInfo.
	/// </summary>
	public class KeyInfo
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="KeyInfo"/> class.
		/// </summary>
		public KeyInfo()
        {
            X509Data = new X509Data();
        }

		#endregion Constructors

		#region Propriedades

		/// <summary>
		/// XS20 - Grupo X509
		/// </summary>
		/// <value>The X509 data.</value>
		[DFeElement("X509Data", Id = "XS20")]
		public X509Data X509Data { get; set; }

		#endregion Propriedades
	}
}