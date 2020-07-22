// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 05-04-2016
//
// Last Modified By : RFTD
// Last Modified On : 08-06-2018
// ***********************************************************************
// <copyright file="DFeBaseAttribute.cs" company="ACBr.Net">
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
using ACBr.Net.DFe.Core.Attributes;
using ACBr.Net.DFe.Core.Serializer;

namespace ACBr.Net.DFe.Core
{
    /// <summary>
    /// Interface IDFeElement
    /// </summary>
    public abstract class DFeBaseAttribute : Attribute
    {
        #region Constructors

        protected DFeBaseAttribute()
        {
            Tipo = TipoCampo.Str;
            Id = "";
            Name = string.Empty;
            Min = 0;
            Max = 0;
            Ocorrencia = 0;
            Ordem = 0;
            Descricao = string.Empty;
        }

        #endregion Constructors

        #region Properties

        public TipoCampo Tipo { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Descricao { get; set; }

        public int Ordem { get; set; }

        public int Max { get; set; }

        public int Min { get; set; }

        public Ocorrencia Ocorrencia { get; set; }

        #endregion Properties
    }
}