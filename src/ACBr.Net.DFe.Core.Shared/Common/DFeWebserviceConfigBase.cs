// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 01-31-2016
//
// Last Modified By : RFTD
// Last Modified On : 06-07-2016
// ***********************************************************************
// <copyright file="DFeWebserviceConfigBase.cs" company="ACBr.Net">
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

namespace ACBr.Net.DFe.Core.Common
{
    public abstract class DFeWebserviceConfigBase<TParent>
    where TParent : ACBrComponent
    {
        #region Constructor

        /// <summary>
        /// Inicializa uma nova instancia da classe <see cref="DFeWebserviceConfigBase{TParent}"/>.
        /// </summary>
        protected DFeWebserviceConfigBase(TParent parent)
        {
            Guard.Against<ArgumentNullException>(parent == null, nameof(parent));
            Parent = parent;

            Ambiente = DFeTipoAmbiente.Homologacao;
            AjustaAguardaConsultaRet = false;
            AguardarConsultaRet = 1;
            Tentativas = 3;
            IntervaloTentativas = 1000;
            ProxyHost = string.Empty;
            ProxyPass = string.Empty;
            ProxyPort = string.Empty;
            ProxyUser = string.Empty;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Componente DFe parente desta configuração.
        /// </summary>
        [Browsable(false)]
        public TParent Parent { get; protected set; }

        /// <summary>
        /// Define/retorna se deve ou não salvar os arquivos soap.
        /// </summary>
        [Browsable(false)]
        public bool Salvar { get; set; }

        /// <summary>
        /// Gets or sets the ambiente.
        /// </summary>
        /// <value>The ambiente.</value>
        [Browsable(true)]
        [DefaultValue(DFeTipoAmbiente.Homologacao)]
        public DFeTipoAmbiente Ambiente { get; set; }

        /// <summary>
        /// Gets the ambiente codigo.
        /// </summary>
        /// <value>The ambiente codigo.</value>
        [Browsable(true)]
        public int AmbienteCodigo => (int)Ambiente;

        /// <summary>
        /// Gets or sets the proxy host.
        /// </summary>
        /// <value>The proxy host.</value>
        [Browsable(true)]
        public string ProxyHost { get; set; }

        /// <summary>
        /// Gets or sets the proxy port.
        /// </summary>
        /// <value>The proxy port.</value>
        [Browsable(true)]
        public string ProxyPort { get; set; }

        /// <summary>
        /// Gets or sets the proxy user.
        /// </summary>
        /// <value>The proxy user.</value>
        [Browsable(true)]
        public string ProxyUser { get; set; }

        /// <summary>
        /// Gets or sets the proxy pass.
        /// </summary>
        /// <value>The proxy pass.</value>
        [Browsable(true)]
        public string ProxyPass { get; set; }

        /// <summary>
        /// Gets or sets the aguardar consulta ret.
        /// </summary>
        /// <value>The aguardar consulta ret.</value>
        [Browsable(true)]
        public uint AguardarConsultaRet { get; set; }

        /// <summary>
        /// Gets or sets the tentativas.
        /// </summary>
        /// <value>The tentativas.</value>
        [Browsable(true)]
        [DefaultValue(3)]
        public int Tentativas { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Browsable(true)]
        [DefaultValue(1000)]
        public int IntervaloTentativas { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [ajusta aguarda consulta ret].
        /// </summary>
        /// <value><c>true</c> if [ajusta aguarda consulta ret]; otherwise, <c>false</c>.</value>
        [Browsable(true)]
        [DefaultValue(false)]
        public bool AjustaAguardaConsultaRet { get; set; }

        #endregion Properties
    }
}