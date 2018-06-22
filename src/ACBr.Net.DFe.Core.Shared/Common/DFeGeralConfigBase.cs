// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 01-31-2016
//
// Last Modified By : RFTD
// Last Modified On : 06-07-2016
// ***********************************************************************
// <copyright file="DFeGeralConfigBase.cs" company="ACBr.Net">
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
    /// <summary>
    ///
    /// </summary>
    public abstract class DFeGeralConfigBase<TParent, [EnumConstraint]TVersaoDFe> : DFeGeralConfigBase<TParent>
    where TParent : ACBrComponent
    where TVersaoDFe : struct
    {
        #region Constructor

        /// <summary>
        /// Inicializa uma nova instancia da classe <see cref="DFeGeralConfigBase{TParent, TVersaoDFe}"/>.
        /// </summary>
        protected DFeGeralConfigBase(TParent parent) : base(parent)
        {
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Define/retorna a versão do documento DFe.
        /// </summary>
        [Browsable(true)]
        public TVersaoDFe VersaoDFe { get; set; }

        #endregion Properties
    }

    public abstract class DFeGeralConfigBase<TParent> where TParent : ACBrComponent
    {
        #region Constructor

        /// <summary>
        /// Inicializa uma nova instancia da classe <see cref="DFeGeralConfigBase{TParent, TVersaoDFe}"/>.
        /// </summary>
        protected DFeGeralConfigBase(TParent parent)
        {
            Guard.Against<ArgumentNullException>(parent == null, nameof(parent));

            Parent = parent;
            TipoEmissao = DFeTipoEmissao.Normal;
            Salvar = true;
            ExibirErroSchema = true;
            FormatoAlerta = "TAG:%TAG% ID:%ID%/%TAG%(%DESCRICAO%) - %MSG%.";
            RetirarAcentos = true;
            RetirarEspacos = true;
            IdentarXml = false;
            ValidarDigest = false;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Componente DFe parente desta configuração.
        /// </summary>
        [Browsable(false)]
        public TParent Parent { get; protected set; }

        /// <summary>
        /// Define/retorna o tipo de emissão.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(DFeTipoEmissao.Normal)]
        public DFeTipoEmissao TipoEmissao { get; set; }

        /// <summary>
        /// Define/retorna se deve ser salvo os arquivos gerais, ou seja, arquivos de envio e
        /// de retorno sem validade jurídica.
        /// </summary>
        /// <value><c>true</c> para salvar; caso contrário, <c>false</c>.</value>
        [Browsable(true)]
        [DefaultValue(true)]
        public bool Salvar { get; set; }

        /// <summary>
        /// Define/retorna se deve exibir os erros de validação do Schema na Execption.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(true)]
        public bool ExibirErroSchema { get; set; }

        /// <summary>
        /// Define/retorna o formato do alerta do serializer.
        /// Valor Padrão = TAG:%TAG% ID:%ID%/%TAG%(%DESCRICAO%) - %MSG%.
        /// </summary>
        [Browsable(true)]
        [DefaultValue("TAG:%TAG% ID:%ID%/%TAG%(%DESCRICAO%) - %MSG%.")]
        public string FormatoAlerta { get; set; }

        /// <summary>
        /// Define/retorna se deve retirar acentos do xml antes de enviar.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(true)]
        public bool RetirarAcentos { get; set; }

        /// <summary>
        /// Define/retorna se deve ser retirado os espaços na hora de gerar o xml.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(true)]
        public bool RetirarEspacos { get; set; }

        /// <summary>
        /// Define/retorna se deve identar o xml na hora de gerar.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        public bool IdentarXml { get; set; }

        /// <summary>
        /// Define/retorna se deve ser validado o digest.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(true)]
        public bool ValidarDigest { get; set; }

        #endregion Properties
    }
}