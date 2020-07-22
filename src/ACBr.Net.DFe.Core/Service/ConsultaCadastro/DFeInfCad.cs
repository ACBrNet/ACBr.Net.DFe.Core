// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 06-30-2018
//
// Last Modified By : RFTD
// Last Modified On : 06-30-2018
// ***********************************************************************
// <copyright file="DFeInfCad.cs" company="ACBr.Net">
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
using System.ComponentModel;
using ACBr.Net.Core.Extensions;
using ACBr.Net.Core.Generics;
using ACBr.Net.DFe.Core.Attributes;
using ACBr.Net.DFe.Core.Common;
using ACBr.Net.DFe.Core.Serializer;

namespace ACBr.Net.DFe.Core.Service
{
    /// <summary>
    /// Informações cadastrais do contribuinte consultado.
    /// </summary>
    public sealed class DFeInfCad : GenericClone<DFeInfCad>, INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Constructors

        public DFeInfCad()
        {
            Endereco = new DFeCadEndereco();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Define/retorna o número da Inscrição Estadual do contribuinte.
        /// </summary>
        [DFeElement(TipoCampo.Custom, "IE", Min = 1, Max = 14, Ocorrencia = Ocorrencia.Obrigatoria)]
        public string IE { get; set; }

        /// <summary>
		/// Define/retorna o número do CNPJ  do contribuinte.
		/// </summary>
        [DFeElement(TipoCampo.StrNumberFill, "CNPJ", Min = 14, Max = 14, Ocorrencia = Ocorrencia.NaoObrigatoria)]
        public string CNPJ { get; set; }

        /// <summary>
		/// Define/retorna o número do CPF do contribuinte.
		/// </summary>
        [DFeElement(TipoCampo.StrNumberFill, "CPF", Min = 14, Max = 14, Ocorrencia = Ocorrencia.NaoObrigatoria)]
        public string CPF { get; set; }

        /// <summary>
		/// Define/retorna a sigla da UF de localização do contribuinte.
		/// Em algumas situações, a UF de localização pode ser diferente da UF consultada.
		/// Ex. IE de Substituto Tributário.
		/// </summary>
        [DFeElement(TipoCampo.Enum, "UF", Min = 1, Max = 2, Ocorrencia = Ocorrencia.Obrigatoria)]
        public DFeSiglaUF UF { get; set; }

        /// <summary>
        /// Define/retorna a situação cadastral do contribuinte.
        /// </summary>
        [DFeElement(TipoCampo.Custom, "cSit", Min = 1, Max = 2, Ocorrencia = Ocorrencia.Obrigatoria)]
        public bool CSit { get; set; }

        /// <summary>
        /// Define/retorna o indicador de contribuinte credenciado a emitir NF-e.
        /// </summary>
        [DFeElement(TipoCampo.Enum, "indCredNFe", Min = 1, Max = 1, Ocorrencia = Ocorrencia.Obrigatoria)]
        public DFeIndCred IndCredNFe { get; set; }

        /// <summary>
        /// Define/retorna o indicador de contribuinte credenciado a emitir CT-e.
        /// </summary>
        [DFeElement(TipoCampo.Enum, "indCredCTe", Min = 1, Max = 1, Ocorrencia = Ocorrencia.Obrigatoria)]
        public DFeIndCred IndCredCTe { get; set; }

        /// <summary>
		/// Define/retorna a razão social ou nome do contribuinte.
		/// </summary>
        [DFeElement(TipoCampo.Str, "xNome", Min = 1, Max = 60, Ocorrencia = Ocorrencia.Obrigatoria)]
        public string XNome { get; set; }

        /// <summary>
		/// Define/retorna o nome fantasia ou nome do contribuinte.
		/// </summary>
        [DFeElement(TipoCampo.Str, "xFant", Min = 1, Max = 60, Ocorrencia = Ocorrencia.NaoObrigatoria)]
        public string XFant { get; set; }

        /// <summary>
		/// Define/retorna o regime de apuração do ICMS.
		/// </summary>
        [DFeElement(TipoCampo.Str, "xRegApur", Min = 1, Max = 60, Ocorrencia = Ocorrencia.NaoObrigatoria)]
        public string XRegApur { get; set; }

        /// <summary>
		/// Define/retorna o CNAE fiscal do contribuinte.
		/// </summary>
        [DFeElement(TipoCampo.Int, "CNAE", Ocorrencia = Ocorrencia.NaoObrigatoria)]
        public int? CNAE { get; set; }

        /// <summary>
        /// Define/retorna a data de início de atividades do contribuinte.
        /// </summary>
        [DFeElement(TipoCampo.Dat, "dIniAtiv", Min = 1, Max = 10, Ocorrencia = Ocorrencia.NaoObrigatoria)]
        public DateTime? DIniAtiv { get; set; }

        /// <summary>
        /// Define/retorna a data da última modificação da situação cadastral do contribuinte.
        /// </summary>
        [DFeElement(TipoCampo.Dat, "dUltSit", Min = 1, Max = 10, Ocorrencia = Ocorrencia.NaoObrigatoria)]
        public DateTime? DUltSit { get; set; }

        /// <summary>
        /// Define/retorna a data de ocorrência da baixa do contribuinte.
        /// </summary>
        [DFeElement(TipoCampo.Dat, "dBaixa", Min = 1, Max = 10, Ocorrencia = Ocorrencia.NaoObrigatoria)]
        public DateTime? DBaixa { get; set; }

        /// <summary>
        /// Define/retorna a inscrição estadual única.
        /// </summary>
        [DFeElement(TipoCampo.StrNumber, "IEUnica", Ocorrencia = Ocorrencia.NaoObrigatoria)]
        public string IEUnica { get; set; }

        /// <summary>
        /// Define/retorna a inscrição estadual atual.
        /// </summary>
        [DFeElement(TipoCampo.StrNumber, "IEAtual", Ocorrencia = Ocorrencia.NaoObrigatoria)]
        public string IEAtual { get; set; }

        /// <summary>
		/// Define/retorna o endereço.
		/// </summary>
        [DFeElement("ender", Ocorrencia = Ocorrencia.NaoObrigatoria)]
        public DFeCadEndereco Endereco { get; set; }

        #endregion Properties

        #region Methods

        private bool ShouldSerializeCPF()
        {
            return CNPJ.IsEmpty();
        }

        private bool ShouldSerializeCNPJ()
        {
            return CPF.IsEmpty();
        }

        private bool ShouldSerializeEndereco()
        {
            return Endereco != null &&
                   (!Endereco.XLgr.IsEmpty() ||
                    !Endereco.Nro.IsEmpty() ||
                    !Endereco.XCpl.IsEmpty() ||
                    !Endereco.XBairro.IsEmpty() ||
                    Endereco.CMun.HasValue ||
                    !Endereco.XMun.IsEmpty() ||
                    !Endereco.CEP.IsEmpty());
        }

        private string SerializeIE()
        {
            return IE.Trim().ToUpper() == "ISENTO" ? IE.Trim().ToUpper() : IE.OnlyNumbers();
        }

        private object DeserializeIE(string value)
        {
            return value;
        }

        private string SerializeCSit()
        {
            return CSit ? "1" : "0";
        }

        private object DeserializeCSit(string value)
        {
            return Convert.ToBoolean(Convert.ToInt32(value));
        }

        #endregion Methods
    }
}