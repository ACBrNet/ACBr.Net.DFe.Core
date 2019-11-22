// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 05-04-2018
//
// Last Modified By : RFTD
// Last Modified On : 05-11-2018
// ***********************************************************************
// <copyright file="DFeDictionaryKeyAttribute.cs" company="ACBr.Net">
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
using ACBr.Net.DFe.Core.Serializer;

namespace ACBr.Net.DFe.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DFeDictionaryKeyAttribute : DFeBaseAttribute
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DFeDictionaryKeyAttribute" /> class.
        /// </summary>
        public DFeDictionaryKeyAttribute()
        {
            Tipo = TipoCampo.Str;
            Id = "";
            Name = string.Empty;
            Min = 0;
            Max = 0;
            Ocorrencia = 0;
            Descricao = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DFeDictionaryKeyAttribute" /> class.
        /// </summary>
        /// <param name="tag">Nome da tag.</param>
        public DFeDictionaryKeyAttribute(string tag) : this()
        {
            Name = tag;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DFeDictionaryKeyAttribute" /> class.
        /// </summary>
        /// <param name="tipo">The tipo.</param>
        /// <param name="name">The name.</param>
        public DFeDictionaryKeyAttribute(TipoCampo tipo, string name) : this()
        {
            Tipo = tipo;
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DFeDictionaryKeyAttribute" /> class.
        /// </summary>
        /// <param name="tipo">The tipo.</param>
        /// <param name="name">The name.</param>
        public DFeDictionaryKeyAttribute(TipoCampo tipo, string name, bool asAttribute) : this()
        {
            Tipo = tipo;
            Name = name;
            AsAttribute = asAttribute;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DFeDictionaryKeyAttribute" /> class.
        /// </summary>
        public DFeDictionaryKeyAttribute(string tag, bool asAttribute) : this()
        {
            Name = tag;
            AsAttribute = asAttribute;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the name space.
        /// </summary>
        /// <value>The name space.</value>
        public string Namespace { get; set; }

        /// <summary>
        /// Gets or sets the AsAttribute.
        /// </summary>
        /// <value>The AsAttribute.</value>
        public bool AsAttribute { get; set; }

        #endregion Properties
    }
}