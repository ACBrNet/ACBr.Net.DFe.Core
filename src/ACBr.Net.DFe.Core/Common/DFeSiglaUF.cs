// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 01-31-2016
//
// Last Modified By : RFTD
// Last Modified On : 06-07-2016
// ***********************************************************************
// <copyright file="DFeSiglaUF.cs" company="ACBr.Net">
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

namespace ACBr.Net.DFe.Core.Common
{
    /// <summary>
    /// Estados brasileiros por sigla
    /// </summary>
    public enum DFeSiglaUF
    {
        /// <summary>
        /// AC- ACre
        /// </summary>
        [DFeEnum("AC")]
        [Description("Acre")]
        AC,

        /// <summary>
        /// AL - Alagoas
        /// </summary>
        [DFeEnum("AL")]
        [Description("Alagoas")]
        AL,

        /// <summary>
        /// AM - Amazonas
        /// </summary>
        [DFeEnum("AM")]
        [Description("Amazonas")]
        AM,

        /// <summary>
        /// AP - Amapá
        /// </summary>
        [DFeEnum("AP")]
        [Description("Amapá")]
        AP,

        /// <summary>
        /// BA - Bahia
        /// </summary>
        [DFeEnum("BA")]
        [Description("Bahia")]
        BA,

        /// <summary>
        /// CE - Ceará
        /// </summary>
        [DFeEnum("CE")]
        [Description("Ceará")]
        CE,

        /// <summary>
        /// DF - Distrito Federal
        /// </summary>
        [DFeEnum("DF")]
        [Description("Distrito Federal")]
        DF,

        /// <summary>
        /// ES - Espírito Santo
        /// </summary>
        [DFeEnum("ES")]
        [Description("Espírito Santo")]
        ES,

        /// <summary>
        /// GO - Goiás
        /// </summary>
        [DFeEnum("GO")]
        [Description("Goiás")]
        GO,

        /// <summary>
        /// MA - Maranhão
        /// </summary>
        [DFeEnum("MA")]
        [Description("Maranhão")]
        MA,

        /// <summary>
        /// MG - Minas Gerais
        /// </summary>
        [DFeEnum("MG")]
        [Description("Minas Gerais")]
        MG,

        /// <summary>
        /// MS - Mato Grosso do Sul
        /// </summary>
        [DFeEnum("MS")]
        [Description("Mato Grosso do Sul")]
        MS,

        /// <summary>
        /// MT - Mato Grosso
        /// </summary>
        [DFeEnum("MT")]
        [Description("Mato Grosso")]
        MT,

        /// <summary>
        /// PA - Pará
        /// </summary>
        [DFeEnum("PA")]
        [Description("Pará")]
        PA,

        /// <summary>
        /// PB - Paraíba
        /// </summary>
        [DFeEnum("PB")]
        [Description("Paraíba")]
        PB,

        /// <summary>
        /// PE - Pernambuco
        /// </summary>
        [DFeEnum("PE")]
        [Description("Pernambuco")]
        PE,

        /// <summary>
        /// PI - Piauí
        /// </summary>
        [DFeEnum("PI")]
        [Description("Piauí")]
        PI,

        /// <summary>
        /// PR - Paraná
        /// </summary>
        [DFeEnum("PR")]
        [Description("Paraná")]
        PR,

        /// <summary>
        /// RJ - Rio de Janeiro
        /// </summary>
        [DFeEnum("RJ")]
        [Description("Rio de Janeiro")]
        RJ,

        /// <summary>
        /// RN - Rio Grande do Norte
        /// </summary>
        [DFeEnum("RN")]
        [Description("Rio Grande do Norte")]
        RN,

        /// <summary>
        /// RO - Rondônia
        /// </summary>
        [DFeEnum("RO")]
        [Description("Rondônia")]
        RO,

        /// <summary>
        /// RR - Roraima
        /// </summary>
        [DFeEnum("RR")]
        [Description("Roraima")]
        RR,

        /// <summary>
        /// RS - Rio Grande do Sul
        /// </summary>
        [DFeEnum("RS")]
        [Description("Rio Grande do Sul")]
        RS,

        /// <summary>
        /// SC - Santa Catarina
        /// </summary>
        [DFeEnum("SC")]
        [Description("Santa Catarina")]
        SC,

        /// <summary>
        /// SE - Sergipe
        /// </summary>
        [DFeEnum("SE")]
        [Description("Sergipe")]
        SE,

        /// <summary>
        /// SP - São Paulo
        /// </summary>
        [DFeEnum("SP")]
        [Description("São Paulo")]
        SP,

        /// <summary>
        /// TO - Tocantins
        /// </summary>
        [DFeEnum("TO")]
        [Description("Tocantins")]
        TO,

        /// <summary>
        /// AN - Ambiente Nacional
        /// </summary>
        [DFeEnum("AN")]
        [Description("Ambiente Nacional")]
        AN = 91,

        /// <summary>
        /// EX - Exterior
        /// </summary>
        [DFeEnum("EX")]
        [Description("Exterior")]
        EX,

        /// <summary>
        /// SU - Suframa
        /// </summary>
        [DFeEnum("SU")]
        [Description("Suframa")]
        SU
    }
}