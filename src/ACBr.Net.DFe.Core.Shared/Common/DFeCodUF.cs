// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 01-31-2016
//
// Last Modified By : RFTD
// Last Modified On : 06-07-2016
// ***********************************************************************
// <copyright file="DFeCodUF.cs" company="ACBr.Net">
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
    /// Estados brasileiros por c�digo
    /// </summary>
    public enum DFeCodUF
    {
        /// <summary>
        /// 12 - Acre
        /// </summary>
        [DFeEnum("12")]
        [Description("Acre")]
        AC = 12,

        /// <summary>
        /// 27 - Alagoas
        /// </summary>
        [DFeEnum("27")]
        [Description("Alagoas")]
        AL = 27,

        /// <summary>
        /// 13 - Amazonas
        /// </summary>
        [DFeEnum("13")]
        [Description("Amazonas")]
        AM = 13,

        /// <summary>
        /// 16 - Amap�
        /// </summary>
        [DFeEnum("16")]
        [Description("Amap�")]
        AP = 16,

        /// <summary>
        /// 29 - Bahia
        /// </summary>
        [DFeEnum("29")]
        [Description("Bahia")]
        BA = 29,

        /// <summary>
        /// 23 - Cear�
        /// </summary>
        [DFeEnum("23")]
        [Description("Cear�")]
        CE = 23,

        /// <summary>
        /// 53 - Distrito Federal
        /// </summary>
        [DFeEnum("53")]
        [Description("Distrito Federal")]
        DF = 53,

        /// <summary>
        /// 32 - Esp�rito Santo
        /// </summary>
        [DFeEnum("32")]
        [Description("Esp�rito Santo")]
        ES = 32,

        /// <summary>
        /// 53 - Goi�s
        /// </summary>
        [DFeEnum("52")]
        [Description("Goi�s")]
        GO = 52,

        /// <summary>
        /// 21 - Maranh�o
        /// </summary>
        [DFeEnum("21")]
        [Description("Maranh�o")]
        MA = 21,

        /// <summary>
        /// 31 - Minas Gerais
        /// </summary>
        [DFeEnum("31")]
        [Description("Minas Gerais")]
        MG = 31,

        /// <summary>
        /// 50 - Mato Grosso do Sul
        /// </summary>
        [DFeEnum("50")]
        [Description("Mato Grosso do Sul")]
        MS = 50,

        /// <summary>
        /// 51 - Mato Grosso
        /// </summary>
        [DFeEnum("51")]
        [Description("Mato Grosso")]
        MT = 51,

        /// <summary>
        /// 15 - Par�
        /// </summary>
        [DFeEnum("15")]
        [Description("Par�")]
        PA = 15,

        /// <summary>
        /// 25 - Para�ba
        /// </summary>
        [DFeEnum("25")]
        [Description("Para�ba")]
        PB = 25,

        /// <summary>
        /// 26 - Pernambuco
        /// </summary>
        [DFeEnum("26")]
        [Description("Pernambuco")]
        PE = 26,

        /// <summary>
        /// 22 - Piau�
        /// </summary>
        [DFeEnum("22")]
        [Description("Piau�")]
        PI = 22,

        /// <summary>
        /// 41 - Paran�
        /// </summary>
        [DFeEnum("41")]
        [Description("Paran�")]
        PR = 41,

        /// <summary>
        /// 33 - Rio de Janeiro
        /// </summary>
        [DFeEnum("33")]
        [Description("Rio de Janeiro")]
        RJ = 33,

        /// <summary>
        /// 24 - Rio Grande do Norte
        /// </summary>
        [DFeEnum("24")]
        [Description("Rio Grande do Norte")]
        RN = 24,

        /// <summary>
        /// 11 - Rond�nia
        /// </summary>
        [DFeEnum("11")]
        [Description("Rond�nia")]
        RO = 11,

        /// <summary>
        /// 14 - Roraima
        /// </summary>
        [DFeEnum("14")]
        [Description("Roraima")]
        RR = 14,

        /// <summary>
        /// 43 - Rio Grande do Sul
        /// </summary>
        [DFeEnum("43")]
        [Description("Rio Grande do Sul")]
        RS = 43,

        /// <summary>
        /// 42 - Santa Catarina
        /// </summary>
        [DFeEnum("42")]
        [Description("Santa Catarina")]
        SC = 42,

        /// <summary>
        /// 28 - Sergipe
        /// </summary>
        [DFeEnum("28")]
        [Description("Sergipe")]
        SE = 28,

        /// <summary>
        /// 35 - S�o Paulo
        /// </summary>
        [DFeEnum("35")]
        [Description("S�o Paulo")]
        SP = 35,

        /// <summary>
        /// 17 - Tocantins
        /// </summary>
        [DFeEnum("17")]
        [Description("Tocantins")]
        TO = 17,

        /// <summary>
        /// 91 - Ambiente Nacional
        /// </summary>
        [DFeEnum("91")]
        [Description("Ambiente Nacional")]
        AN = 91,

        /// <summary>
        /// 00 - Exterior
        /// </summary>
        [DFeEnum("00")]
        [Description("Exterior")]
        EX = 0,

        /// <summary>
        /// 99 - Suframa
        /// </summary>
        [DFeEnum("99")]
        [Description("Suframa")]
        SU
    }
}