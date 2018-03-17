// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 03-11-2018
//
// Last Modified By : RFTD
// Last Modified On : 03-11-2018
// ***********************************************************************
// <copyright file="DFeConfigBase.cs" company="ACBr.Net">
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
using ACBr.Net.Core;
using ACBr.Net.Core.Exceptions;
using ExtraConstraints;

namespace ACBr.Net.DFe.Core.Common
{
    [TypeConverter(typeof(ACBrExpandableObjectConverter))]
    public abstract class DFeConfigBase<TParent, TGeralConfig, [EnumConstraint]TVersaoDFe, TWebserviceConfig,
        TCertificadosConfig, TArquivosConfig, [EnumConstraint]TSchemas>
        where TParent : ACBrComponent
        where TGeralConfig : DFeGeralConfigBase<TParent, TVersaoDFe>
        where TVersaoDFe : struct
        where TWebserviceConfig : DFeWebserviceConfigBase<TParent>
        where TCertificadosConfig : DFeCertificadosConfigBase<TParent>
        where TArquivosConfig : DFeArquivosConfigBase<TParent, TSchemas>
        where TSchemas : struct
    {
        #region Constructors

        /// <summary>
        /// Inicializa uma nova instancia da classe <see cref="DFeConfigBase{TParent, TGeralConfig, TWebserviceConfig, TCertificadosConfig, TArquivosConfig, TVersaoDFe, TSchemas}"/>.
        /// </summary>
        /// <param name="parent"></param>
        protected DFeConfigBase(TParent parent)
        {
            Guard.Against<ArgumentNullException>(parent == null, nameof(parent));

            Parent = parent;
            CreateConfigs();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Componente DFe parente desta configuração.
        /// </summary>
        [Browsable(false)]
        public TParent Parent { get; protected set; }

        /// <summary>
        /// Configurações principais do componente.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TGeralConfig Geral { get; protected set; }

        /// <summary>
        /// Configurações de webservices do componente.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TWebserviceConfig WebServices { get; protected set; }

        /// <summary>
        /// Configurações de certificado do componente.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TCertificadosConfig Certificados { get; protected set; }

        /// <summary>
        /// Configurações de arquivos do componente.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TArquivosConfig Arquivos { get; protected set; }

        #endregion Properties

        #region Methods

        protected abstract void CreateConfigs();

        #endregion Methods
    }
}