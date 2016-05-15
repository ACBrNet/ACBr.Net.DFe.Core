// ***********************************************************************
// Assembly         : ACBr.Net.Core
// Author           : arezende
// Created          : 07-27-2014
//
// Last Modified By : RFTD
// Last Modified On : 09-02-2014
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using System.Xml.Schema;
using ACBr.Net.Core.Exceptions;
using ACBr.Net.Core.Extensions;

namespace ACBr.Net.DFe.Core
{
    /// <summary>
    /// Classe CertificadoDigital.
    /// </summary>
    public static class CertificadoDigital
    {
		#region Methods

		/// <summary>
		/// Assina o XML usando o certificado informado.
		/// </summary>
		/// <param name="xml">A NFe.</param>
		/// <param name="element">A Url.</param>
		/// <param name="pCertificado">O certificado.</param>
		/// <returns>System.String.</returns>
		/// <exception cref="System.Exception">Erro ao efetuar assinatura digital, detalhes:  + ex.Message</exception>
		public static string SignXml(string xml, string element, X509Certificate2 pCertificado)
        {
            try
            {
                // Load the certificate from the certificate store.
                var cert = pCertificado;

                // Create a new XML document.
                var doc = new XmlDocument();
                doc.LoadXml(xml);

                // Format the document to ignore white spaces.
                doc.PreserveWhitespace = false;
                
                // Create a SignedXml object.
                var signedXml = new SignedXml(doc) { SigningKey = cert.PrivateKey };

                // Add the key to the SignedXml document.

                // Create a reference to be signed.
                var reference = new Reference();

                // pega o uri que deve ser assinada
                var xmlNode = doc.GetElementsByTagName(element).Item(0);
	            var uri = xmlNode?.Attributes;
	            if (uri != null)
		            foreach (var atributo in uri.Cast<XmlAttribute>().Where(atributo => atributo.Name == "Id"))
			            reference.Uri = "#" + atributo.InnerText;

	            // Add an enveloped transformation to the reference.
                var env = new XmlDsigEnvelopedSignatureTransform();
                reference.AddTransform(env);

                var c14 = new XmlDsigC14NTransform();
                reference.AddTransform(c14);

                // Add the reference to the SignedXml object.
                signedXml.AddReference(reference);

                // Create a new KeyInfo object.
                var keyInfo = new KeyInfo();

                // Load the certificate into a KeyInfoX509Data object
                // and add it to the KeyInfo object.
                keyInfo.AddClause(new KeyInfoX509Data(cert));

                // Add the KeyInfo object to the SignedXml object.
                signedXml.KeyInfo = keyInfo;

                // Compute the signature.
                signedXml.ComputeSignature();

                // Get the XML representation of the signature and save
                // it to an XmlElement object.
                var xmlDigitalSignature = signedXml.GetXml();

                // Append the element to the XML document.
                doc.DocumentElement?.AppendChild(doc.ImportNode(xmlDigitalSignature, true));
                
                if (doc.FirstChild is XmlDeclaration)
                    doc.RemoveChild(doc.FirstChild);

                return doc.AsString();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao efetuar assinatura digital, detalhes: " + ex.Message);
            }
        }

		/// <summary>
		/// Assina os elementos do XML usando o certificado informado.
		/// </summary>
		/// <param name="xml">A NFe.</param>
		/// <param name="element">A Url.</param>
		/// <param name="pCertificado">O certificado.</param>
		/// <returns>System.String.</returns>
		/// <exception cref="System.Exception">Erro ao efetuar assinatura digital, detalhes:  + ex.Message</exception>
		public static string SignElements(string xml, string element, X509Certificate2 pCertificado)
		{
			try
			{
				// Load the certificate from the certificate store.
				var cert = pCertificado;

				// Create a new XML document.
				var doc = new XmlDocument();
				doc.LoadXml(xml);

				// Format the document to ignore white spaces.
				doc.PreserveWhitespace = false;

				// Create a SignedXml object.
				var signedXml = new SignedXml(doc) { SigningKey = cert.PrivateKey };
				
				// pega o uri que deve ser assinada
				foreach (var xmlNode in doc.GetElementsByTagName(element).Cast<XmlNode>())
				{
					// Create a reference to be signed.
					var reference = new Reference();
					var uri = xmlNode?.Attributes;
					if (uri != null)
						foreach (var atributo in uri.Cast<XmlAttribute>().Where(atributo => atributo.Name == "Id"))
							reference.Uri = "#" + atributo.InnerText;

					// Add an enveloped transformation to the reference.
					var env = new XmlDsigEnvelopedSignatureTransform();
					reference.AddTransform(env);

					var c14 = new XmlDsigC14NTransform();
					reference.AddTransform(c14);

					// Add the reference to the SignedXml object.
					signedXml.AddReference(reference);

					// Create a new KeyInfo object.
					var keyInfo = new KeyInfo();

					// Load the certificate into a KeyInfoX509Data object
					// and add it to the KeyInfo object.
					keyInfo.AddClause(new KeyInfoX509Data(cert));

					// Add the KeyInfo object to the SignedXml object.
					signedXml.KeyInfo = keyInfo;

					// Compute the signature.
					signedXml.ComputeSignature();

					// Get the XML representation of the signature and save
					// it to an XmlElement object.
					var xmlDigitalSignature = signedXml.GetXml();
					xmlNode.AddTag(xmlDigitalSignature);
				}

				if (doc.FirstChild is XmlDeclaration)
					doc.RemoveChild(doc.FirstChild);

				return doc.AsString();
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao efetuar assinatura digital, detalhes: " + ex.Message);
			}
		}

		/// <summary>
		/// Busca certificados instalado se informado uma serie
		/// se não abre caixa de dialogos de certificados.
		/// </summary>
		/// <param name="cerSerie">Serie do certificado.</param>
		/// <returns>X509Certificate2.</returns>
		/// <exception cref="System.Exception">
		/// Nenhum certificado digital foi selecionado ou o certificado selecionado está com problemas.
		/// or
		/// Certificado digital não encontrado
		/// or
		/// </exception>
		public static X509Certificate2 SelecionarCertificado(string cerSerie)
        {
            var certificate = new X509Certificate2();
	        try
            {
	            var store = new X509Store("MY", StoreLocation.CurrentUser);
                store.Open(OpenFlags.OpenExistingOnly);
                var certificates = store.Certificates.Find(X509FindType.FindByTimeValid, DateTime.Now, true)
                    .Find(X509FindType.FindByKeyUsage, X509KeyUsageFlags.DigitalSignature, true);

				X509Certificate2Collection certificatesSel;
	            if (cerSerie.IsEmpty())
                {
                    certificatesSel = X509Certificate2UI.SelectFromCollection(certificates, "Certificados Digitais", "Selecione o Certificado Digital para uso no aplicativo", X509SelectionFlag.SingleSelection);
                    if (certificatesSel.Count == 0)
                    {
                        certificate.Reset();
                        throw new ACBrDFeException("Nenhum certificado digital foi selecionado ou o certificado selecionado está com problemas.");
                    }

                    certificate = certificatesSel[0];
                }
                else
                {
                    certificatesSel = certificates.Find(X509FindType.FindBySerialNumber, cerSerie, true);
                    if (certificatesSel.Count == 0)
                    {
                        certificate.Reset();
                        throw new ACBrDFeException("Certificado digital não encontrado");
                    }

                    certificate = certificatesSel[0];
                }

                store.Close();
                return certificate;
            }
            catch (Exception ex)
            {
                throw new ACBrDFeException("Erro ao selecionar o certificado", ex);
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

            var cert = new X509Certificate2(caminho, senha, X509KeyStorageFlags.MachineKeySet);
            return cert;
        }

        /// <summary>
        /// Validars the XML.
        /// </summary>
        /// <param name="arquivoXml">The arquivo XML.</param>
        /// <param name="schemaNf">The schema nf.</param>
        /// <param name="erros">The erro.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool ValidarXml(string arquivoXml, string schemaNf, out string[] erros)
        {
			var errorList = new List<string>();
			if (arquivoXml.IsEmpty())
            {
                errorList.Add("Arquivo da nota fiscal não encontrado.");
                erros = errorList.ToArray();
	            return false;
            }

            if (!File.Exists(schemaNf))
            {
                errorList.Add("Arquivo de Schema não encontrado.");
                erros = errorList.ToArray();
	            return false;
            }

			var settings = new XmlReaderSettings();
			settings.ValidationEventHandler += (sender, args) =>
			{
				switch (args.Severity)
				{
					case XmlSeverityType.Warning:
						errorList.Add("Nenhum arquivo de Schema foi encontrado para efetuar a validação...");
						break;

					case XmlSeverityType.Error:
						errorList.Add("Ocorreu um erro durante a validação....");
						break;

					default:
						throw new ArgumentOutOfRangeException();
				}

				// Erro na validação do schema XSD
				if (args.Exception != null)
				{
					errorList.Add("\nErro: " + args.Exception.Message + "\nLinha " + args.Exception.LinePosition + " - Coluna " + args.Exception.LineNumber + "\nSource: " + args.Exception.SourceUri);
				}
			};

			try
            {
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas.Add("http://www.portalfiscal.inf.br/nfe", XmlReader.Create(schemaNf));
                var xml = new StringReader(arquivoXml);
                using (var reader = XmlReader.Create(xml, settings))
                {
                    while (reader.Read())
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                errorList.Add(ex.Message);
                erros = errorList.ToArray();
                errorList = null;
                return false;

            }

            erros = errorList.ToArray();
            return erros.Length < 1;
        }

        #endregion Methods
    }
}
