﻿// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 08-10-2018
//
// Last Modified By : RFTD
// Last Modified On : 08-10-2018
// ***********************************************************************
// <copyright file="DFeReportClass.cs" company="ACBr.Net">
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
using ACBr.Net.Core;
using ACBr.Net.Core.Extensions;
using ACBr.Net.Core.Logging;

#if !NETSTANDARD2_0

using System.Drawing;

#endif

namespace ACBr.Net.DFe.Core.Common
{
    [TypeConverter(typeof(ACBrExpandableObjectConverter))]
    public abstract class DFeReportClass<TParent> : ACBrComponent, IACBrLog
        where TParent : ACBrComponent
    {
        #region Fields

        private TParent parent;
        private string nomeArquivo;

        #endregion Fields

        #region Constructors

        protected DFeReportClass(TParent parent)
        {
            Parent = parent;
        }

        #endregion Constructors

        #region Properties

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TParent Parent
        {
            get => parent;
            set
            {
                if (parent == value) return;

                parent = value;
                ParentChanged();
            }
        }

#if !NETSTANDARD2_0
        public Image Logo { get; set; }
#else
        public byte[] Logo { get; set; }
#endif

        public FiltroDFeReport Filtro { get; set; }

        public bool MostrarPreview { get; set; }

        public bool MostrarSetup { get; set; }

        public bool UsarPathPDF { get; set; }

        public string PrinterName { get; set; }

        public int NumeroCopias { get; set; }

        public string NomeArquivo { get; set; }

        public string SoftwareHouse { get; set; }

        public string Site { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Imprime o documento DFe.
        /// </summary>
        public abstract void Imprimir();

        /// <summary>
        /// Imprime o documento DFe em PDF.
        /// </summary>
        public abstract void ImprimirPDF();

        /// <summary>
        /// Função executada toda vez que é mudado o Parent.
        /// </summary>
        protected abstract void ParentChanged();

        #endregion Methods
    }
}