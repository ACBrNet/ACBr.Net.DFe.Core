// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 06-30-2018
//
// Last Modified By : RFTD
// Last Modified On : 06-30-2018
// ***********************************************************************
// <copyright file="DFeCadEndereco.cs" company="ACBr.Net">
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
using ACBr.Net.Core.Generics;
using ACBr.Net.DFe.Core.Attributes;
using ACBr.Net.DFe.Core.Serializer;

namespace ACBr.Net.DFe.Core.Service
{
    /// <summary>
    /// Tipo Dados do Endereço
    /// </summary>
    public sealed class DFeCadEndereco : GenericClone<DFeCadEndereco>, INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Properties

        /// <summary>
        /// Define/retorna o logradouro.
        /// </summary>
        [DFeElement(TipoCampo.Str, "xLgr", Min = 2, Max = 255, Ocorrencia = Ocorrencia.NaoObrigatoria)]
        public string XLgr { get; set; }

        /// <summary>
        /// Define/retorna o número.
        /// </summary>
        [DFeElement(TipoCampo.Str, "nro", Min = 1, Max = 60, Ocorrencia = Ocorrencia.NaoObrigatoria)]
        public string Nro { get; set; }

        /// <summary>
        /// Define/retorna o complemento.
        /// </summary>
        [DFeElement(TipoCampo.Str, "xCpl", Min = 1, Max = 60, Ocorrencia = Ocorrencia.NaoObrigatoria)]
        public string XCpl { get; set; }

        /// <summary>
        /// Define/retorna o bairro.
        /// </summary>
        [DFeElement(TipoCampo.Str, "xBairro", Min = 1, Max = 60, Ocorrencia = Ocorrencia.NaoObrigatoria)]
        public string XBairro { get; set; }

        /// <summary>
        /// Define/retorna o código do município (utilizar a tabela do IBGE), informar 9999999 para operações com o exterior.
        /// </summary>
        [DFeElement(TipoCampo.Int, "cMun", Min = 7, Max = 7, Ocorrencia = Ocorrencia.NaoObrigatoria)]
        public int? CMun { get; set; }

        /// <summary>
        /// Define/retorna o nome do município.
        /// </summary>
        [DFeElement(TipoCampo.Str, "xMun", Min = 2, Max = 60, Ocorrencia = Ocorrencia.NaoObrigatoria)]
        public string XMun { get; set; }

        /// <summary>
        /// Define/retorna o CEP.
        /// </summary>
        [DFeElement(TipoCampo.StrNumber, "CEP", Min = 8, Max = 8, Ocorrencia = Ocorrencia.NaoObrigatoria)]
        public string CEP { get; set; }

        #endregion Properties
    }
}