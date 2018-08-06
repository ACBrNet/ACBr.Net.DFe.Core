// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 07-08-2018
//
// Last Modified By : RFTD
// Last Modified On : 07-08-2018
// ***********************************************************************
// <copyright file="DFeSignInfoElement.cs" company="ACBr.Net">
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

namespace ACBr.Net.DFe.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class DFeSignInfoElement : Attribute
    {
        #region Constructors

        /// <summary>
        /// Inicializa uma nova intância da classe <see cref="DFeSignInfoElement"/>.
        /// </summary>
        public DFeSignInfoElement()
        {
            SignElement = string.Empty;
            SignAtribute = "Id";
        }

        /// <summary>
        /// Inicializa uma nova intância da classe <see cref="DFeSignInfoElement"/>.
        /// </summary>
        /// <param name="signElement">O elemento a ser assinado.</param>
        public DFeSignInfoElement(string signElement)
        {
            SignElement = signElement;
            SignAtribute = "Id";
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Define/retorna o elemento a ser assinado.
        /// </summary>
        /// <value>The sign element.</value>
        public string SignElement { get; set; }

        /// <summary>
        /// Define/retorna o atributo identificador do elemento a ser assinado.
        /// </summary>
        /// <value>The sign atribute.</value>
        public string SignAtribute { get; set; }

        #endregion Properties
    }
}