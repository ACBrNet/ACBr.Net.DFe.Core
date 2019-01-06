// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 06-30-2018
//
// Last Modified By : RFTD
// Last Modified On : 06-30-2018
// ***********************************************************************
// <copyright file="DFeConsultaCadastroServiceClient.cs" company="ACBr.Net">
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
using System.Text;
using System.Xml;
using ACBr.Net.Core;
using ACBr.Net.Core.Exceptions;
using ACBr.Net.Core.Extensions;
using ACBr.Net.DFe.Core.Common;
using ACBr.Net.DFe.Core.Extensions;

using ACBr.Net.DFe.Core.Extensions;

namespace ACBr.Net.DFe.Core.Service
{
    public abstract class DFeConsultaCadastroServiceClient<TDFeConfig, TParent, TGeralConfig, TVersaoDFe,
         TWebserviceConfig, TCertificadosConfig, TArquivosConfig, TSchemas> :
        DFeServiceClient<TDFeConfig, TParent, TGeralConfig, TVersaoDFe, TWebserviceConfig,
            TCertificadosConfig, TArquivosConfig, TSchemas, IDFeCadConsultaCadastro>
        where TDFeConfig : DFeConfigBase<TParent, TGeralConfig, TVersaoDFe,
             TWebserviceConfig, TCertificadosConfig, TArquivosConfig, TSchemas>
        where TParent : ACBrComponent
        where TGeralConfig : DFeGeralConfigBase<TParent, TVersaoDFe>
        where TVersaoDFe : Enum
        where TWebserviceConfig : DFeWebserviceConfigBase<TParent>
        where TCertificadosConfig : DFeCertificadosConfigBase<TParent>
        where TArquivosConfig : DFeArquivosConfigBase<TParent, TSchemas>
        where TSchemas : Enum
    {
        #region Constructors

        protected DFeConsultaCadastroServiceClient(TDFeConfig config, string url, X509Certificate2 certificado = null) :
            base(config, url, certificado)
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Consulta o cadastro do contribuinte.
        /// </summary>
        /// <param name="uf"></param>
        /// <param name="ufConsulta"></param>
        /// <param name="cpfCNPJ"></param>
        /// <param name="IE"></param>
        /// <returns></returns>
        public DFeConsultaCadastroResposta ConsultaCadastro(DFeSiglaUF uf, DFeCodUF ufConsulta, string cpfCNPJ, string IE)
        {
            Guard.Against<ArgumentNullException>(!Enum.IsDefined(typeof(DFeCodUF), uf), nameof(uf));
            Guard.Against<ArgumentNullException>(cpfCNPJ.IsEmpty() && IE.IsEmpty(), nameof(IE));

            lock (serviceLock)
            {
                var request = new StringBuilder();
                request.Append("<ConsCad xmlns=\"http://www.portalfiscal.inf.br/nfe\" versao=\"2.00\">");
                request.Append("<infCons>");
                request.Append("<xServ>CONS-CAD</xServ>");
                request.Append($"<UF>{uf.GetDFeValue()}</UF>");
                if (cpfCNPJ.IsEmpty())
                    request.Append($"<IE>{IE}</IE>");
                else
                    request.Append(cpfCNPJ.IsCNPJ() ? $"<CNPJ>{cpfCNPJ}</CNPJ>" : $"<CPF>{cpfCNPJ}</CPF>");
                request.Append("</infCons>");
                request.Append("</ConsCad>");

                var dadosMsg = request.ToString();
                ValidateMessage(dadosMsg);

                var doc = new XmlDocument();
                doc.LoadXml(dadosMsg);

                var cabecalho = new DFeWsCabecalho
                {
                    CUf = ufConsulta.GetDFeValue().ToInt32(),
                    VersaoDados = "2.00"
                };
                var inValue = new ConsultaCadastroRequest(cabecalho, doc);
                var retVal = Channel.ConsultaCadastro(inValue);

                var retorno =
                    new DFeConsultaCadastroResposta(dadosMsg, retVal.Result.OuterXml, EnvelopeSoap, RetornoWS);
                return retorno;
            }
        }

        #endregion Properties
    }
}