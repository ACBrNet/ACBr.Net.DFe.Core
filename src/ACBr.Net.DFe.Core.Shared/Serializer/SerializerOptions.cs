// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 05-04-2016
//
// Last Modified By : RFTD
// Last Modified On : 05-04-2016
// ***********************************************************************
// <copyright file="SerializerOptions.cs" company="ACBr.Net">
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

using System.Collections.Generic;
using System.Text;

namespace ACBr.Net.DFe.Core.Serializer
{
    /// <summary>
    /// Class SerializerOptions. This class cannot be inherited.
    /// </summary>
    public class SerializerOptions
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DFeSerializer" /> class.
        /// </summary>
        internal SerializerOptions()
        {
            ErrosAlertas = new List<string>();
            FormatoAlerta = "TAG:%TAG% ID:%ID%/%TAG%(%DESCRICAO%) - %MSG%.";
            RemoverAcentos = false;
            FormatarXml = true;
            Encoding = Encoding.UTF8;
        }

        #endregion Constructors

        #region Propriedades

        public bool RemoverAcentos { get; set; }

        public bool RemoverEspacos { get; set; }

        public bool FormatarXml { get; set; }

        public bool OmitirDeclaracao { get; set; }

        public Encoding Encoding { get; set; }

        public List<string> ErrosAlertas { get; }

        public string FormatoAlerta { get; set; }

        #endregion Propriedades

        #region Methods

        internal void AddAlerta(string id, string tag, string descricao, string alerta)
        {
            // O Formato da mensagem de erro pode ser alterado pelo usuario alterando-se a property FormatoAlerta: onde;
            // %TAG%       : Representa a TAG; ex: <nLacre>
            // %ID%        : Representa a ID da TAG; ex X34
            // %MSG%       : Representa a mensagem de alerta
            // %DESCRICAO% : Representa a Descrição da TAG

            var s = FormatoAlerta.Clone() as string;
            if (s == null)
                return;

            s = s.Replace("%ID%", id).Replace("%TAG%", $"<{tag}>").Replace("%DESCRICAO%", descricao).Replace("%MSG%", alerta);

            ErrosAlertas.Add(s);
        }

        #endregion Methods
    }
}