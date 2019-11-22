// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 06-30-2018
//
// Last Modified By : RFTD
// Last Modified On : 06-30-2018
// ***********************************************************************
// <copyright file="DFeInfCons.cs" company="ACBr.Net">
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
using ACBr.Net.DFe.Core.Collection;
using ACBr.Net.DFe.Core.Common;
using ACBr.Net.DFe.Core.Serializer;

namespace ACBr.Net.DFe.Core.Service
{
    public sealed class DFeInfCons : GenericClone<DFeInfCons>, INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Constructors

        public DFeInfCons()
        {
            InfCad = new DFeCollection<DFeInfCad>();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
		/// Define/retorna a versão do Aplicativo que processou o pedido de consulta de cadastro.
		/// </summary>
        [DFeElement(TipoCampo.Str, "verAplic", Min = 1, Max = 20, Ocorrencia = Ocorrencia.Obrigatoria)]
        public string VersaoAplicacao { get; set; }

        /// <summary>
		/// Define/retorna o código do status da mensagem enviada.
		/// </summary>
        [DFeElement(TipoCampo.Int, "cStat", Min = 1, Max = 3, Ocorrencia = Ocorrencia.Obrigatoria)]
        public int CStat { get; set; }

        /// <summary>
		/// Define/retorna a descrição literal do status do serviço solicitado.
		/// </summary>
        [DFeElement(TipoCampo.Str, "xMotivo", Min = 1, Max = 255, Ocorrencia = Ocorrencia.Obrigatoria)]
        public string Motivo { get; set; }

        /// <summary>
		/// Define/retorna a sigla da UF consultada, utilizar SU para SUFRAMA.
		/// </summary>
        [DFeElement(TipoCampo.Str, "UF", Min = 1, Max = 2, Ocorrencia = Ocorrencia.Obrigatoria)]
        public string UF { get; set; }

        /// <summary>
		/// Define/retorna o número da Inscrição Estadual do contribuinte.
		/// </summary>
        [DFeElement(TipoCampo.StrNumber, "IE", Min = 2, Max = 14, Ocorrencia = Ocorrencia.NaoObrigatoria)]
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
		/// Define/retorna a data da Consulta.
		/// </summary>
        [DFeElement(TipoCampo.DatHorTz, "dhCons", Min = 1, Max = 2, Ocorrencia = Ocorrencia.Obrigatoria)]
        public DateTimeOffset DhCons { get; set; }

        /// <summary>
		/// Define/retorna o código da UF de atendimento.
		/// </summary>
        [DFeElement(TipoCampo.Enum, "cUF", Min = 1, Max = 2, Ocorrencia = Ocorrencia.Obrigatoria)]
        public DFeCodUF CodUF { get; set; }

        /// <summary>
        /// Define/retorna as informações cadastrais do contribuinte consultado.
        /// </summary>
        [DFeCollection("infCad", MinSize = 0, Ocorrencia = Ocorrencia.NaoObrigatoria)]
        public DFeCollection<DFeInfCad> InfCad { get; set; }

        #endregion Properties

        #region Methods

        private bool ShouldSerializeCPF()
        {
            return CNPJ.IsEmpty() && IE.IsEmpty();
        }

        private bool ShouldSerializeCNPJ()
        {
            return CPF.IsEmpty() && IE.IsEmpty();
        }

        private bool ShouldSerializeIE()
        {
            return CPF.IsEmpty() && CNPJ.IsEmpty();
        }

        #endregion Methods
    }
}