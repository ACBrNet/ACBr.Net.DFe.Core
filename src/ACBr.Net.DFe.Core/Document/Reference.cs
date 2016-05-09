// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 05-07-2016
//
// Last Modified By : RFTD
// Last Modified On : 05-07-2016
// ***********************************************************************
// <copyright file="Reference.cs" company="ACBr.Net">
//     Copyright © ACBr.Net 2014 - 2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using ACBr.Net.DFe.Core.Attributes;
using ACBr.Net.DFe.Core.Serializer;

namespace ACBr.Net.DFe.Core.Document
{
	/// <summary>
	/// Class Reference.
	/// </summary>
	public class Reference
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Reference"/> class.
		/// </summary>
		public Reference()
		{
			Transforms = new List<Transform>();
			DigestMethod = new DigestMethod();
		}
		
		#endregion Constructors

		#region Propriedades

		/// <summary>
		/// XS08 - Atributo URI da tag Reference
		/// </summary>
		/// <value>The URI.</value>
		[DFeAttribute(TipoCampo.Str, "URI", Id = "XS08", Min = 0, Max = 999, Ocorrencias = 1)]
		public string URI { get; set; }

		/// <summary>
		/// XS10 - Grupo do algorithm de Transform
		/// </summary>
		/// <value>The transforms.</value>
		[DFeElement("Transforms", Id = "XS10")]
		[DFeItem("Transform")]
        public List<Transform> Transforms { get; set; }

		/// <summary>
		/// XS15 - Grupo do Método de DigestMethod
		/// </summary>
		/// <value>The digest method.</value>
		[DFeElement("DigestMethod", Id = "XS15")]
		public DigestMethod DigestMethod { get; set; }

		/// <summary>
		/// XS17 - Digest Value (Hash SHA-1 – Base64)
		/// </summary>
		/// <value>The digest value.</value>
		[DFeElement(TipoCampo.Str, "DigestValue", Id = "XS17", Min = 0, Max = 999, Ocorrencias = 1)]
		public string DigestValue { get; set; }

		#endregion Propriedades
	}
}