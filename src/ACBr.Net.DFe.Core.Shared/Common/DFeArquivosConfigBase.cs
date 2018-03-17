// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 01-31-2016
//
// Last Modified By : RFTD
// Last Modified On : 06-07-2016
// ***********************************************************************
// <copyright file="DFeArquivosConfigBase.cs" company="ACBr.Net">
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
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using ACBr.Net.Core;
using ACBr.Net.Core.Exceptions;
using ACBr.Net.Core.Extensions;
using ExtraConstraints;

namespace ACBr.Net.DFe.Core.Common
{
    public abstract class DFeArquivosConfigBase<TParent, [EnumConstraint]TSchemas>
        where TParent : ACBrComponent
        where TSchemas : struct
    {
        #region Fields

        private string pathSalvar;
        private string arquivoServicos;
        private string pathSchemas;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Inicializa uma nova instancia da classe <see cref="DFeArquivosConfigBase{TParent, TSchemas}"/>.
        /// </summary>
        protected DFeArquivosConfigBase(TParent parent)
        {
            Guard.Against<ArgumentNullException>(parent == null, nameof(parent));

            Parent = parent;
            PathSalvar = string.Empty;
            PathSchemas = string.Empty;
            arquivoServicos = string.Empty;

            Salvar = true;
            AdicionarLiteral = false;
            SepararPorCNPJ = false;
            SepararPorModelo = false;
            SepararPorAno = false;
            SepararPorMes = false;
            SepararPorDia = false;

            OrdenacaoPath = new List<TagOrdenacaoPath>();
            SchemasCache = new Dictionary<TSchemas, string>();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Componente DFe parente desta configuração.
        /// </summary>
        [Browsable(false)]
        public TParent Parent { get; protected set; }

        /// <summary>
        /// Define/retorna o caminho onde deve ser salvo os arquivos.
        /// </summary>
        [Browsable(true)]
        public string PathSalvar
        {
            get
            {
                if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return pathSalvar;

                if (pathSalvar.IsEmpty())
                {
                    pathSalvar = Path.Combine(Assembly.GetExecutingAssembly().GetPath(), "Docs");
                }

                return pathSalvar;
            }
            set => pathSalvar = value;
        }

        /// <summary>
        /// Define/retorna o caminho onde estão so schemas.
        /// </summary>
        [Browsable(true)]
        public string PathSchemas
        {
            get
            {
                if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return pathSchemas;

                if (pathSchemas.IsEmpty())
                {
                    pathSchemas = Path.Combine(Assembly.GetExecutingAssembly().GetPath(), "Schemas");
                }

                return pathSchemas;
            }
            set => pathSchemas = value;
        }

        /// <summary>
        /// Define/retorna o arquivo com os dados dos serviços.
        /// </summary>
        [Browsable(true)]
        public string ArquivoServicos
        {
            get => arquivoServicos;
            set
            {
                if (value == arquivoServicos) return;

                arquivoServicos = value ?? string.Empty;
                ArquivoServicoChange();
            }
        }

        /// <summary>
        /// Define/retorna se deve salvar os arquivos xml, trata-se de arquivos com validade jurídica.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(true)]
        public bool Salvar { get; set; }

        /// <summary>
        /// Define/retorna se deve ser adicionado um literal ao caminho de salvamento.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        public bool AdicionarLiteral { get; set; }

        /// <summary>
        /// Define/retorna se deve ser adicionado o CNPJ ao caminho de salvamento.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        public bool SepararPorCNPJ { get; set; }

        /// <summary>
        /// Define/retorna se deve ser adicionado o numero do
        /// modelo do arquivo DFe ao caminho de salvamento.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        public bool SepararPorModelo { get; set; }

        /// <summary>
        /// Define/retorna se deve ser adicionado o ano ao caminho de salvamento.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        public bool SepararPorAno { get; set; }

        /// <summary>
        /// Define/retorna se deve ser adicionado o mês ao caminho de salvamento.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        public bool SepararPorMes { get; set; }

        /// <summary>
        /// Define/retorna se deve ser adicionado o dia ao caminho de salvamento.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        public bool SepararPorDia { get; set; }

        /// <summary>
        /// Retorna a ordem de criação dos caminhos para salvamento dos arquivos.
        /// </summary>
        [Browsable(false)]
        public List<TagOrdenacaoPath> OrdenacaoPath { get; }

        /// <summary>
        /// Retorna um dicionario que contem o cache dos paths de cada schema lido.
        /// </summary>
        [Browsable(false)]
        public Dictionary<TSchemas, string> SchemasCache { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Metodo chamado quando muda o caminho do arquivo de serviços.
        /// </summary>
        protected abstract void ArquivoServicoChange();

        /// <summary>
        /// Metodo que retorna o caminho para o tipo de schema solicitado.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public abstract string GetSchema(TSchemas schema);

        /// <summary>
        /// Gera um path de salvamento.
        /// </summary>
        /// <param name="aPath"></param>
        /// <param name="aLiteral"></param>
        /// <param name="cnpj"></param>
        /// <param name="data"></param>
        /// <param name="modeloDescr"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        protected virtual string GetPath(string aPath, string aLiteral, string cnpj = "", DateTime? data = null, string modeloDescr = "")
        {
            var dir = aPath.IsEmpty() ? PathSalvar : aPath;

            if (!OrdenacaoPath.Any())
            {
                if (SepararPorCNPJ) OrdenacaoPath.Add(TagOrdenacaoPath.CNPJ);
                if (SepararPorModelo) OrdenacaoPath.Add(TagOrdenacaoPath.Modelo);
                if (SepararPorAno || SepararPorMes || SepararPorDia) OrdenacaoPath.Add(TagOrdenacaoPath.Data);
                if (AdicionarLiteral) OrdenacaoPath.Add(TagOrdenacaoPath.Literal);
            }

            foreach (var ordenacaoPath in OrdenacaoPath)
            {
                switch (ordenacaoPath)
                {
                    case TagOrdenacaoPath.CNPJ:

                        if (!cnpj.IsEmpty())
                            dir = Path.Combine(dir, cnpj.OnlyNumbers());
                        break;

                    case TagOrdenacaoPath.Modelo:
                        if (!modeloDescr.IsEmpty())
                            dir = Path.Combine(dir, modeloDescr);
                        break;

                    case TagOrdenacaoPath.Data:
                        if (!data.HasValue) data = DateTime.Now;

                        if (SepararPorAno)
                            dir = Path.Combine(dir, data.Value.ToString("yyyy"));

                        if (SepararPorMes)
                        {
                            dir = SepararPorAno ? Path.Combine(dir, data.Value.ToString("MM")) :
                                                  Path.Combine(dir, data.Value.ToString("yyyy"), data.Value.ToString("MM"));

                            if (SepararPorDia)
                                dir = Path.Combine(dir, data.Value.ToString("dd"));
                        }

                        break;

                    case TagOrdenacaoPath.Literal:
                        if (!aLiteral.IsEmpty())
                        {
                            if (!dir.ToLower().Contains(aLiteral.ToLower()))
                                dir = Path.Combine(dir, aLiteral);
                        }
                        break;
                }
            }

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            return dir;
        }

        #endregion Methods
    }
}