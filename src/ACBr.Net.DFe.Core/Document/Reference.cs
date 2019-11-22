// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 05-07-2016
//
// Last Modified By : RFTD
// Last Modified On : 05-07-2016
// ***********************************************************************
// <copyright file="Reference.cs" company="ACBr.Net">
//		        		   The MIT License (MIT)
//	     		    Copyright (c) 2016 Grupo ACBr.Net
//
//	 Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
//	 The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//	 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary></summary>
// ***********************************************************************
using ACBr.Net.DFe.Core.Attributes;
using ACBr.Net.DFe.Core.Collection;
using ACBr.Net.DFe.Core.Serializer;

namespace ACBr.Net.DFe.Core.Document
{
    public sealed class Reference
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Reference"/> class.
        /// </summary>
        public Reference()
        {
            Transforms = new DFeCollection<Transform>();
            DigestMethod = new DigestMethod();
        }

        #endregion Constructors

        #region Propriedades

        /// <summary>
        /// XS08 - Atributo URI da tag Reference
        /// </summary>
        /// <value>The URI.</value>
        [DFeAttribute(TipoCampo.Str, "URI", Id = "XS08", Min = 0, Max = 999, Ocorrencia = Ocorrencia.Obrigatoria)]
        public string URI { get; set; }

        /// <summary>
        /// XS10 - Grupo do algorithm de Transform
        /// </summary>
        /// <value>The transforms.</value>
        [DFeCollection("Transforms", Id = "XS10")]
        [DFeItem(typeof(Transform), "Transform")]
        public DFeCollection<Transform> Transforms { get; set; }

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
        [DFeElement(TipoCampo.Str, "DigestValue", Id = "XS17", Min = 0, Max = 999, Ocorrencia = Ocorrencia.Obrigatoria)]
        public string DigestValue { get; set; }

        #endregion Propriedades
    }
}