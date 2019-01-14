// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 12-01-2019
//
// Last Modified By : RFTD
// Last Modified On : 12-01-2019
// ***********************************************************************
// <copyright file="DFeDistribuicaoServiceClient.cs" company="ACBr.Net">
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
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;
using ACBr.Net.Core;
using ACBr.Net.Core.Exceptions;
using ACBr.Net.Core.Extensions;
using ACBr.Net.DFe.Core.Common;
using ACBr.Net.DFe.Core.Extensions;

namespace ACBr.Net.DFe.Core.Service
{
    public abstract class DFeDistribuicaoServiceClient<TDFeConfig, TParent, TGeralConfig, TVersaoDFe,
         TWebserviceConfig, TCertificadosConfig, TArquivosConfig, TSchemas> :
        DFeServiceClient<TDFeConfig, TParent, TGeralConfig, TVersaoDFe, TWebserviceConfig,
            TCertificadosConfig, TArquivosConfig, TSchemas, IRequestChannel>
        where TDFeConfig : DFeConfigBase<TParent, TGeralConfig, TVersaoDFe, TWebserviceConfig, TCertificadosConfig, TArquivosConfig, TSchemas>
        where TParent : ACBrComponent
        where TGeralConfig : DFeGeralConfigBase<TParent, TVersaoDFe>
        where TVersaoDFe : Enum
        where TWebserviceConfig : DFeWebserviceConfigBase<TParent>
        where TCertificadosConfig : DFeCertificadosConfigBase<TParent>
        where TArquivosConfig : DFeArquivosConfigBase<TParent, TSchemas>
        where TSchemas : Enum
    {
        #region Constructors

        protected DFeDistribuicaoServiceClient(TDFeConfig config, string url, string soapAction, string ns,
            DFeCodUF? uf = null, X509Certificate2 certificado = null, string tagGrupoMensagem = "", string versao = "1.00",
            string tagConsultaChave = "", string tagChave = "") : base(config, url, certificado)
        {
            Guard.Against<ArgumentNullException>(soapAction.IsEmpty(), "Informe o ação SOAP.");
            Guard.Against<ArgumentNullException>(ns.IsEmpty(), "Informe o Namespace do xml.");
            Guard.Against<ArgumentNullException>(versao.IsEmpty(), "Informe a versão do xml.");
            Guard.Against<ArgumentException>(uf.HasValue && uf.IsIn(DFeCodUF.EX, DFeCodUF.AN, DFeCodUF.SU), "Estado informado incorreto.");

            SoapAction = soapAction;
            Versao = versao;
            Namespace = ns;
            UF = uf;
            TagConsultaChave = tagConsultaChave;
            TagChave = tagChave;
            TagGrupoMensagem = tagGrupoMensagem;
            ArquivoEnvio = "-con-dist-dfe";
            ArquivoResposta = "-con-dist-dfe";
        }

        #endregion Constructors

        #region Properties

        public string SoapAction { get; protected set; }

        public string Versao { get; protected set; }

        public string Namespace { get; protected set; }

        public DFeCodUF? UF { get; protected set; }

        public string TagConsultaChave { get; protected set; }

        public string TagChave { get; protected set; }

        public string TagGrupoMensagem { get; protected set; }

        #endregion Properties

        #region Methods

        public DistribuicaoDFeResponse ConsultaDFe(string cpfCNPJ, string nsu = "", string chave = "", string ultNsu = "")
        {
            Guard.Against<ArgumentNullException>(cpfCNPJ.IsEmpty() || !cpfCNPJ.IsCPFOrCNPJ(), "CPF/CNPJ informado invalido ou vazio.");
            Guard.Against<ArgumentNullException>(nsu.IsEmpty() && chave.IsEmpty() && ultNsu.IsEmpty(), "Informe ao menos 1 parâmetro.");

            lock (serviceLock)
            {
                var request = new StringBuilder();
                if (!TagGrupoMensagem.IsEmpty())
                    request.Append($"<{TagGrupoMensagem}>");

                request.Append($"<distDFeInt xmlns=\"{Namespace}\" versao=\"{Versao}\">");
                request.Append($"<tpAmb>{Configuracoes.WebServices.Ambiente.GetDFeValue()}</tpAmb>");
                if (UF.HasValue)
                    request.Append($"<cUFAutor>{UF.Value.GetDFeValue()}</cUFAutor>");

                request.Append(cpfCNPJ.IsCNPJ() ? $"<CNPJ>{cpfCNPJ}</CNPJ>" : $"<CPF>{cpfCNPJ}</CPF>");

                if (nsu.IsEmpty())
                {
                    if (chave.IsEmpty())
                    {
                        request.Append($"<distNSU><ultNSU>{ultNsu.ZeroFill(15)}</ultNSU></distNSU>");
                    }
                    else
                    {
                        request.AppendFormat("<{0}><{1}>{2}</{1}></{0}>", TagConsultaChave, TagChave, chave.OnlyNumbers());
                    }
                }
                else
                {
                    request.Append($"<consNSU><NSU>{nsu.ZeroFill(15)}</NSU></consNSU>");
                }

                request.Append("</distDFeInt>");

                if (!TagGrupoMensagem.IsEmpty())
                    request.Append($"</{TagGrupoMensagem}>");

                var dadosMsg = request.ToString();
                ValidateMessage(dadosMsg);

                var doc = new XmlDocument();
                doc.LoadXml(dadosMsg);

                var message = Message.CreateMessage(Endpoint.Binding.MessageVersion, SoapAction, doc.DocumentElement);

                var ret = Channel.Request(message);
                Guard.Against<ACBrDFeException>(ret == null, "Nenhum retorno do webservice.");

                var reader = ret.GetReaderAtBodyContents();

                var resultXml = new XmlDocument();
                resultXml.LoadXml(reader.ReadContentAsString());

                var element = resultXml.GetElementsByTagName("retDistDFeInt")[0];

                var response = new DistribuicaoDFeResponse(dadosMsg, element.OuterXml, EnvelopeSoap, RetornoWS);
                return response;
            }
        }

        #endregion Methods
    }
}