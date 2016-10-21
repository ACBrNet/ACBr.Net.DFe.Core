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

using ACBr.Net.Core.Exceptions;
using ACBr.Net.Core.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using System.Xml.Schema;

namespace ACBr.Net.DFe.Core
{
	public static class CertificadoDigital
	{
		#region Methods

		/// <summary>
		/// Assina a XML usando o certificado informado.
		/// </summary>
		/// <param name="xml">O Xml.</param>
		/// <param name="pUri">A Uri da referencia.</param>
		/// <param name="pNode">O node onde sera feito o append da assinatura.</param>
		/// <param name="pCertificado">O certificado.</param>
		/// <param name="comments">Se for <c>true</c> vai inserir a tag #withcomments no transform.</param>
		/// <returns>System.String.</returns>
		/// <exception cref="ACBrDFeException">Erro ao efetuar assinatura digital.</exception>
		/// <exception cref="ACBrDFeException">Erro ao efetuar assinatura digital.</exception>
		public static string AssinarXml(string xml, string pUri, string pNode, X509Certificate2 pCertificado, bool comments = false)
		{
			try
			{
				var doc = new XmlDocument();
				doc.LoadXml(xml);

				//Adiciona Certificado ao Key Info
				var keyInfo = new KeyInfo();
				keyInfo.AddClause(new KeyInfoX509Data(pCertificado));

				//Seta chaves
				var signedDocument = new SignedXml(doc)
				{
					SigningKey = pCertificado.PrivateKey,
					KeyInfo = keyInfo
				};

				// Cria referencia
				var reference = new Reference { Uri = pUri };

				// Adiciona transformação a referencia
				reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
				reference.AddTransform(new XmlDsigC14NTransform(comments));

				// Adiciona referencia ao xml
				signedDocument.AddReference(reference);

				// Calcula Assinatura
				signedDocument.ComputeSignature();

				// Pega representação da assinatura
				var xmlDigitalSignature = signedDocument.GetXml();

				// Adiciona ao doc XML
				var xmlElement = doc.GetElementsByTagName(pNode).Cast<XmlElement>().FirstOrDefault();
				Guard.Against<ACBrDFeException>(xmlElement == null, "Nome do elemento de assinatura incorreto");

				var element = doc.ImportNode(xmlDigitalSignature, true);
				xmlElement.AppendChild(element);

				return doc.AsString();
			}
			catch (Exception ex)
			{
				throw new ACBrDFeException("Erro ao efetuar assinatura digital.", ex);
			}
		}

		/// <summary>
		/// Assina Multiplos elementos dentro da Xml.
		/// </summary>
		/// <param name="xml">O Xml.</param>
		/// <param name="pUri">A Uri da referencia.</param>
		/// <param name="pNode">O node onde sera feito o append da assinatura.</param>
		/// <param name="pCertificado">O certificado.</param>
		/// <param name="comments">Se for <c>true</c> vai inserir a tag #withcomments no transform.</param>
		/// <returns>System.String.</returns>
		/// <exception cref="ACBrDFeException">Erro ao efetuar assinatura digital.</exception>
		/// <exception cref="ACBrDFeException">Erro ao efetuar assinatura digital.</exception>
		public static string AssinarXmlTodos(string xml, string pUri, string pNode, X509Certificate2 pCertificado, bool comments = false)
		{
			try
			{
				var doc = new XmlDocument();
				doc.LoadXml(xml);

				var xmlElements = doc.GetElementsByTagName(pNode).Cast<XmlElement>().ToArray();
				Guard.Against<ACBrDFeException>(!xmlElements.Any(), "Nome do elemento de assinatura incorreto");

				foreach (var element in xmlElements)
				{
					var signed = AssinarElemento(element.OuterXml, pUri, pNode, pCertificado, comments);
					var signedElement = doc.ImportNode(signed, true);
					element.ParentNode?.ReplaceChild(signedElement, element);
				}

				return doc.AsString();
			}
			catch (Exception ex)
			{
				throw new ACBrDFeException("Erro ao efetuar assinatura digital.", ex);
			}
		}

		private static XmlElement AssinarElemento(string elemento, string pUri, string pNode, X509Certificate2 pCertificado, bool comments = false)
		{
			var doc = new XmlDocument();
			doc.LoadXml(elemento);

			//Adiciona Certificado ao Key Info
			var keyInfo = new KeyInfo();
			keyInfo.AddClause(new KeyInfoX509Data(pCertificado));

			//Seta chaves
			var signedDocument = new SignedXml(doc)
			{
				SigningKey = pCertificado.PrivateKey,
				KeyInfo = keyInfo
			};

			// Cria referencia
			var reference = new Reference { Uri = pUri };

			// Adiciona transformação a referencia
			reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
			reference.AddTransform(new XmlDsigC14NTransform(comments));

			// Adiciona referencia ao xml
			signedDocument.AddReference(reference);

			// Calcula Assinatura
			signedDocument.ComputeSignature();

			// Pega representação da assinatura
			var xmlDigitalSignature = signedDocument.GetXml();

			// Adiciona ao doc XML
			var xmlElement = doc.GetElementsByTagName(pNode).Cast<XmlElement>().FirstOrDefault();
			Guard.Against<ACBrDFeException>(xmlElement == null, "Nome do elemento de assinatura incorreto");

			var element = doc.ImportNode(xmlDigitalSignature, true);
			xmlElement.AppendChild(element);

			return xmlElement;
		}

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
			var store = new X509Store("MY", StoreLocation.CurrentUser);
			store.Open(OpenFlags.MaxAllowed);

			try
			{
				var certificates = store.Certificates.Find(X509FindType.FindByTimeValid, DateTime.Now, true)
					.Find(X509FindType.FindByKeyUsage, X509KeyUsageFlags.DigitalSignature, true);

				X509Certificate2Collection certificadosSelecionados;

				if (cerSerie.IsEmpty())
				{
					certificadosSelecionados = X509Certificate2UI.SelectFromCollection(certificates, "Certificados Digitais",
						"Selecione o Certificado Digital para uso no aplicativo", X509SelectionFlag.SingleSelection);
				}
				else
				{
					certificadosSelecionados = certificates.Find(X509FindType.FindBySerialNumber, cerSerie, true);
					Guard.Against<ACBrDFeException>(certificadosSelecionados.Count == 0, "Certificado digital não encontrado");
				}

				var certificado = certificadosSelecionados.Count < 1 ? null : certificadosSelecionados[0];
				store.Close();
				return certificado;
			}
			catch (Exception ex)
			{
				throw new ACBrDFeException("Erro ao selecionar o certificado", ex);
			}
			finally
			{
				store.Close();
			}
		}

		/// <summary>
		/// Exibi o certificado usando a ui nativa do windows.
		/// </summary>
		/// <param name="certificado"></param>
		public static void ExibirCertificado(this X509Certificate2 certificado)
		{
			Guard.Against<ArgumentNullException>(certificado == null, nameof(certificado));

			X509Certificate2UI.DisplayCertificate(certificado);
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
		/// <param name="schema">The schema nf.</param>
		/// <param name="erros">The erro.</param>
		/// <param name="avisos">The avisos.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		public static bool ValidarXml(string arquivoXml, string schema, out string[] erros, out string[] avisos)
		{
			var errorList = new List<string>();
			var avisosList = new List<string>();

			if (string.IsNullOrEmpty(arquivoXml))
			{
				errorList.Add("Arquivo da nota fiscal não encontrado.");
				erros = errorList.ToArray();
				avisos = avisosList.ToArray();
				return false;
			}

			if (!File.Exists(schema))
			{
				errorList.Add("Arquivo de Schema não encontrado.");
				erros = errorList.ToArray();
				avisos = avisosList.ToArray();
				return false;
			}

			try
			{
				var settings = new XmlReaderSettings();
				var schema2 = XmlSchema.Read(new XmlTextReader(schema), (sender, args) =>
				{
					switch (args.Severity)
					{
						case XmlSeverityType.Warning:
							// ReSharper disable once AccessToModifiedClosure
							avisosList.Add(args.Message);
							break;

						case XmlSeverityType.Error:
							// ReSharper disable once AccessToModifiedClosure
							errorList.Add(args.Message);
							break;
					}

					// Erro na validação do schema XSD
					if (args.Exception != null)
					{
						// ReSharper disable once AccessToModifiedClosure
						errorList.Add("\nErro: " + args.Exception.Message + "\nLinha " + args.Exception.LinePosition + " - Coluna "
								  + args.Exception.LineNumber + "\nSource: " + args.Exception.SourceUri);
					}
				});

				settings.ValidationType = ValidationType.Schema;
				settings.Schemas.Add(schema2);

				var input = new StringReader(arquivoXml);
				using (var reader2 = XmlReader.Create(input, settings))
				{
					while (reader2.Read())
					{
					}
				}
			}
			catch (Exception exception)
			{
				errorList.Add(exception.Message);
			}

			erros = errorList.ToArray();
			avisos = avisosList.ToArray();
			errorList = null;
			avisosList = null;

			return (erros.Length < 1);
		}

		#endregion Methods
	}
}