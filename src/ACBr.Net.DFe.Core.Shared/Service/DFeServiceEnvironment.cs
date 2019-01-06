// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 04-01-2019
//
// Last Modified By : RFTD
// Last Modified On : 04-01-2019
// ***********************************************************************
// <copyright file="DFeServiceEnvironment.cs" company="ACBr.Net">
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

using System;
using System.Collections.Generic;
using System.Linq;
using ACBr.Net.DFe.Core.Attributes;
using ACBr.Net.DFe.Core.Common;
using ACBr.Net.DFe.Core.Serializer;

namespace ACBr.Net.DFe.Core.Service
{
    public class DFeServiceEnvironment<TTIpo> where TTIpo : Enum
    {
        #region Constructors

        public DFeServiceEnvironment()
        {
            Enderecos = new Dictionary<TTIpo, string>();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        ///
        /// </summary>
        /// <param name="tipo"></param>
        [DFeIgnore]
        public string this[TTIpo tipo]
        {
            get { return Enderecos?.SingleOrDefault(x => Equals(x.Key, tipo)).Value ?? string.Empty; }
        }

        [DFeAttribute(TipoCampo.Enum, "Ambiente")]
        public DFeTipoAmbiente Ambiente { get; set; }

        [DFeAttribute(TipoCampo.Enum, "UF")]
        public DFeSiglaUF UF { get; set; }

        [DFeDictionary("Enderecos")]
        [DFeDictionaryKey(TipoCampo.Enum, "Tipo", true)]
        [DFeDictionaryValue(TipoCampo.Str, "Endereco")]
        public Dictionary<TTIpo, string> Enderecos { get; set; }

        #endregion Properties
    }
}