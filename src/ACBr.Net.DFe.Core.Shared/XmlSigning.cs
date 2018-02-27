// ***********************************************************************
// Assembly         : ACBr.Net.Core
// Author           : RFTD
// Created          : 12-27-2017
//
// Last Modified By : RFTD
// Last Modified On : 12-27-2017
// ***********************************************************************
// <copyright file="XmlSigning.cs" company="ACBr.Net">
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
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using ACBr.Net.Core.Exceptions;
using ACBr.Net.Core.Extensions;

namespace ACBr.Net.DFe.Core
{
    /// <summary>
    /// Classe com os metodos para assinatura e validação de assinatura de xml usando Hash Sha1.
    /// </summary>
    public static partial class XmlSigning
    {
        #region Methods

        /// <summary>
        /// Assina a XML usando o certificado informado.
        /// </summary>
        /// <param name="xml">O Xml.</param>
        /// <param name="docElement">O elemento principal do xml a ser assinado.</param>
        /// <param name="infoElement">O elemento a ser assinado.</param>
        /// <param name="pCertificado">O certificado.</param>
        /// <param name="comments">Se for <c>true</c> vai inserir a tag #withcomments no transform.</param>
        /// <param name="digest">Algoritmo usando para gerar o hash por padrão SHA1.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="ACBrDFeException">Erro ao efetuar assinatura digital.</exception>
        /// <exception cref="ACBrDFeException">Erro ao efetuar assinatura digital.</exception>
        public static string AssinarXml(string xml, string docElement, string infoElement, X509Certificate2 pCertificado,
            bool comments = false, SignDigest digest = SignDigest.SHA1)
        {
            try
            {
                var xmlDoc = new XmlDocument { PreserveWhitespace = true };
                xmlDoc.LoadXml(xml);
                AssinarDocumento(xmlDoc, docElement, infoElement, pCertificado, comments);
                return xmlDoc.AsString();
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
        /// <param name="docElement">O elemento principal do xml a ser assinado.</param>
        /// <param name="infoElement">O elemento a ser assinado.</param>
        /// <param name="certificado">O certificado.</param>
        /// <param name="comments">Se for <c>true</c> vai inserir a tag #withcomments no transform.</param>
        /// <param name="digest">Algoritmo usando para gerar o hash por padrão SHA1.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="ACBrDFeException">Erro ao efetuar assinatura digital.</exception>
        /// <exception cref="ACBrDFeException">Erro ao efetuar assinatura digital.</exception>
        public static string AssinarXmlTodos(string xml, string docElement, string infoElement, X509Certificate2 certificado,
            bool comments = false, SignDigest digest = SignDigest.SHA1)
        {
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(xml);

                var xmlElements = doc.GetElementsByTagName(docElement).Cast<XmlElement>()
                    .Where(x => x.GetElementsByTagName(infoElement).Count == 1).ToArray();
                Guard.Against<ACBrDFeException>(!xmlElements.Any(), "Nome do elemento de assinatura incorreto");

                foreach (var element in xmlElements)
                {
                    var xmlDoc = new XmlDocument { PreserveWhitespace = true };
                    xmlDoc.LoadXml(element.OuterXml);
                    AssinarDocumento(xmlDoc, docElement, infoElement, certificado, comments);

                    // ReSharper disable once AssignNullToNotNullAttribute
                    var signedElement = doc.ImportNode(xmlDoc.DocumentElement, true);
                    element.ParentNode?.ReplaceChild(signedElement, element);
                }

                return doc.AsString();
            }
            catch (Exception ex)
            {
                throw new ACBrDFeException("Erro ao efetuar assinatura digital.", ex);
            }
        }

        private static void AssinarDocumento(XmlDocument doc, string docElement, string infoElement,
            X509Certificate2 certificado, bool comments = false, SignDigest digest = SignDigest.SHA1)
        {
            Guard.Against<ArgumentException>(!infoElement.IsEmpty() && doc.GetElementsByTagName(infoElement).Count != 1, "Referencia invalida ou não é unica.");

            //Adiciona Certificado ao Key Info
            var keyInfo = new KeyInfo();
            keyInfo.AddClause(new KeyInfoX509Data(certificado));

            //Seta chaves
            var signedDocument = new SignedXml(doc)
            {
                SigningKey = certificado.PrivateKey,
                KeyInfo = keyInfo,
                SignedInfo =
                {
                    SignatureMethod = GetSignatureMethod(digest)
                }
            };

            var uri = infoElement.IsEmpty() ? "" : $"#{doc.GetElementsByTagName(infoElement)[0].Attributes?["Id"]?.InnerText}";

            // Cria referencia
            var reference = new Reference
            {
                Uri = uri,
                DigestMethod = GetDigestMethod(digest)
            };

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
            var xmlElement = doc.GetElementsByTagName(docElement).Cast<XmlElement>().FirstOrDefault();
            Guard.Against<ACBrDFeException>(xmlElement == null, "Nome do docElemnt incorreto");

            var element = doc.ImportNode(xmlDigitalSignature, true);
            xmlElement.AppendChild(element);
        }

        private static string GetSignatureMethod(SignDigest digest)
        {
            switch (digest)
            {
                case SignDigest.SHA1:
                    return "http://www.w3.org/2000/09/xmldsig#rsa-sha1";

                case SignDigest.SHA256:
                    return "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";

                default:
                    throw new ArgumentOutOfRangeException(nameof(digest), digest, null);
            }
        }

        private static string GetDigestMethod(SignDigest digest)
        {
            switch (digest)
            {
                case SignDigest.SHA1:
                    return "http://www.w3.org/2000/09/xmldsig#sha1";

                case SignDigest.SHA256:
                    return "http://www.w3.org/2001/04/xmlenc#sha256";

                default:
                    throw new ArgumentOutOfRangeException(nameof(digest), digest, null);
            }
        }

        /// <summary>
        /// Validar a assinatura do Xml
        /// </summary>
        /// <param name="doc">o documento xml</param>
        /// <param name="docElement">O elemento principal do xml onde esta a tag Signature.</param>
        /// <returns></returns>
        public static bool ValidarAssinatura(XmlDocument doc, string docElement)
        {
            var signedXml = new SignedXml(doc);

            var element = doc.GetElementsByTagName(docElement).Cast<XmlElement>().FirstOrDefault();

            Guard.Against<ACBrDFeException>(element == null, "Root Node não encontrado.");

            var signElement = element.LastChild as XmlElement;

            Guard.Against<ACBrDFeException>(signElement?.Name != "Signature", "Signature node não encontrado.");

            signedXml.LoadXml(signElement);

            var cspParams = new CspParameters { KeyContainerName = "XML_DSIG_RSA_KEY" };
            var rsaKey = new RSACryptoServiceProvider(cspParams);

            return signedXml.CheckSignature(rsaKey);
        }

        #endregion Methods
    }
}