// ***********************************************************************
// Assembly         : ACBr.Net.Core
// Author           : arezende
// Created          : 07-27-2014
//
// Last Modified By : RFTD
// Last Modified On : 19-01-2017
// ***********************************************************************
// <copyright file="CertificadoDigital.cs" company="ACBr.Net">
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

using ACBr.Net.Core.Exceptions;
using ACBr.Net.Core.Extensions;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace ACBr.Net.DFe.Core
{
    /// <summary>
    /// Classe CertificadoDigital.
    /// </summary>
    public static class CertificadoDigital
    {
        #region Methods

        /// <summary>
        /// Busca certificados instalado se informado uma serie
        /// senão abre caixa de dialogos de certificados.
        /// </summary>
        /// <param name="cerSerie">Serie do certificado.</param>
        /// <returns>X509Certificate2.</returns>
        /// <exception cref="System.Exception">
        /// Nenhum certificado digital foi selecionado ou o certificado selecionado está com problemas.
        /// or
        /// Certificado digital não encontrado
        /// or
        /// </exception>
        public static X509Certificate2 SelecionarCertificado(string cerSerie = "")
        {
#if NETSTANDARD2_0
            var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
#else
            var store = new X509Store("MY", StoreLocation.CurrentUser);
#endif

            try
            {
                store.Open(OpenFlags.MaxAllowed | OpenFlags.ReadOnly);

                var certificates = store.Certificates.Find(X509FindType.FindByTimeValid, DateTime.Now, true)
                    .Find(X509FindType.FindByKeyUsage, X509KeyUsageFlags.DigitalSignature, false);

                X509Certificate2Collection certificadosSelecionados;

                if (cerSerie.IsEmpty())
                {
#if NETSTANDARD2_0
                    throw new ACBrDFeException("Número de série obrigatório.");
#else
                    certificadosSelecionados = X509Certificate2UI.SelectFromCollection(certificates, "Certificados Digitais",
                        "Selecione o Certificado Digital para uso no aplicativo", X509SelectionFlag.SingleSelection);
#endif
                }
                else
                {
                    certificadosSelecionados = certificates.Find(X509FindType.FindBySerialNumber, cerSerie, false);
                    Guard.Against<ACBrDFeException>(certificadosSelecionados.Count == 0, "Certificado digital não encontrado");
                }

                var certificado = certificadosSelecionados.Count < 1 ? null : certificadosSelecionados[0];
                return certificado;
            }
            finally
            {
                store.Close();
            }
        }

        /// <summary>
        /// Seleciona um certificado informando o caminho e a senha.
        /// </summary>
        /// <param name="caminho">O caminho.</param>
        /// <param name="senha">A senha.</param>
        /// <returns>X509Certificate2.</returns>
        /// <exception cref="System.Exception">Arquivo do Certificado digital não encontrado</exception>
        public static X509Certificate2 SelecionarCertificado(string caminho, string senha)
        {
            Guard.Against<ArgumentNullException>(caminho.IsEmpty(), "Caminho do arquivo não poder ser nulo ou vazio !");
            Guard.Against<ArgumentException>(!File.Exists(caminho), "Arquivo do Certificado digital não encontrado !");

            var cert = new X509Certificate2(caminho, senha);
            return cert;
        }

        /// <summary>
        /// Seleciona um certificado passando um array de bytes.
        /// </summary>
        /// <param name="certificado">O certificado.</param>
        /// <param name="senha">A senha.</param>
        /// <returns>X509Certificate2.</returns>
        /// <exception cref="System.Exception">Arquivo do Certificado digital não encontrado</exception>
        public static X509Certificate2 SelecionarCertificado(byte[] certificado, string senha = "")
        {
            Guard.Against<ArgumentNullException>(certificado != null, "O certificado não poder ser nulo ou vazio !");
            Guard.Against<ArgumentException>(certificado.Length > 0, "O tamanhado do certificado não pode ser zero !");

            var cert = new X509Certificate2(certificado, senha);
            return cert;
        }

#if !NETSTANDARD2_0

		/// <summary>
		/// Exibi o certificado usando a ui nativa do windows.
		/// </summary>
		/// <param name="certificado"></param>
		public static void ExibirCertificado(this X509Certificate2 certificado)
		{
			Guard.Against<ArgumentNullException>(certificado == null, nameof(certificado));

			X509Certificate2UI.DisplayCertificate(certificado);
		}

#endif

        #endregion Methods
    }
}