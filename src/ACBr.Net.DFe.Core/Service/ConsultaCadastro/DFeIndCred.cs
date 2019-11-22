// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 06-30-2018
//
// Last Modified By : RFTD
// Last Modified On : 06-30-2018
// ***********************************************************************
// <copyright file="DFeIndCred.cs" company="ACBr.Net">
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

using System.ComponentModel;
using ACBr.Net.DFe.Core.Attributes;

namespace ACBr.Net.DFe.Core.Service
{
    /// <summary>
    /// Indicador de credenciamento.
    /// </summary>
    public enum DFeIndCred
    {
        [DFeEnum("0")]
        [Description("0 - Não credenciado para emissão")]
        NaoCredenciado = 0,

        [DFeEnum("1")]
        [Description("1 - Credenciado")]
        Credenciado = 1,

        [DFeEnum("2")]
        [Description("2 - Credenciado com obrigatoriedade para todas operações")]
        CredenciadoObrigatoriedade = 2,

        [DFeEnum("3")]
        [Description("3 - Credenciado com obrigatoriedade parcial")]
        CredenciadoObrigatoriedadeParcial = 3,

        [DFeEnum("4")]
        [Description("4 – a SEFAZ não fornece a informação")]
        SefazNaoForneceInformacao = 4
    }
}