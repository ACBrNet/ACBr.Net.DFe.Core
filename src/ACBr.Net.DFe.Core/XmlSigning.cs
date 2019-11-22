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
using System.Text;
using System.Xml;
using ACBr.Net.Core.Exceptions;
using ACBr.Net.Core.Extensions;
using ACBr.Net.Core.Logging;
using ACBr.Net.DFe.Core.Attributes;
using ACBr.Net.DFe.Core.Common;
using ACBr.Net.DFe.Core.Document;
using KeyInfo = System.Security.Cryptography.Xml.KeyInfo;
using Reference = System.Security.Cryptography.Xml.Reference;

#if NETFULL

using ACBr.Net.DFe.Core.Cryptography;

#endif

namespace ACBr.Net.DFe.Core
{
    /// <summary>
    /// Classe com os metodos para assinatura e validação de assinatura de xml usando Hash Sha1.
    /// </summary>
    public static class XmlSigning
    {
        #region Constructors

#if NETFULL

        static XmlSigning()
        {
            CryptoConfig.AddAlgorithm(typeof(RSAPKCS1SHA256SignatureDescription), "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256");
        }

#endif

        #endregion Constructors

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
                AssinarDocumento(xmlDoc, docElement, infoElement, "Id", pCertificado, comments);
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
                    AssinarDocumento(xmlDoc, docElement, infoElement, "Id", certificado, comments);

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

        /// <summary>
        /// Assina o xml.
        /// </summary>
        /// <param name="doc">O Xml.</param>
        /// <param name="docElement">O elemento principal do xml a ser assinado.</param>
        /// <param name="infoElement">O elemento a ser assinado.</param>
        /// <param name="signAtribute">O atributo identificador do elemento a ser assinado.</param>
        /// <param name="certificado">O certificado.</param>
        /// <param name="comments">Se for <c>true</c> vai inserir a tag #withcomments no transform.</param>
        /// <param name="digest">Algoritmo usando para gerar o hash por padrão SHA1.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="ACBrDFeException">Erro ao efetuar assinatura digital.</exception>
        /// <exception cref="ACBrDFeException">Erro ao efetuar assinatura digital.</exception>
        public static void AssinarDocumento(XmlDocument doc, string docElement, string infoElement, string signAtribute,
            X509Certificate2 certificado, bool comments = false, SignDigest digest = SignDigest.SHA1)
        {
            Guard.Against<ArgumentNullException>(doc == null, "XmlDOcument não pode ser nulo.");
            Guard.Against<ArgumentException>(docElement.IsEmpty(), "docElement não pode ser nulo ou vazio.");

            var xmlDigitalSignature = GerarAssinatura(doc, infoElement, signAtribute, certificado, comments, digest);
            var xmlElement = doc.GetElementsByTagName(docElement).Cast<XmlElement>().FirstOrDefault();

            Guard.Against<ACBrDFeException>(xmlElement == null, "Elemento principal não encontrado.");

            var element = doc.ImportNode(xmlDigitalSignature, true);
            xmlElement.AppendChild(element);
        }

        /// <summary>
        /// Gera a assinatura do xml e retorna uma instancia da classe <see cref="DFeSignature"/>.
        /// </summary>
        /// <typeparam name="TDocument">The type of the t document.</typeparam>
        /// <param name="document">The document.</param>
        /// <param name="certificado">The certificado.</param>
        /// <param name="comments">if set to <c>true</c> [comments].</param>
        /// <param name="digest">The digest.</param>
        /// <param name="options">The options.</param>
        /// <returns>DFeSignature.</returns>
        public static DFeSignature AssinarDocumento<TDocument>(DFeSignDocument<TDocument> document,
            X509Certificate2 certificado, bool comments, SignDigest digest,
            DFeSaveOptions options, out string signedXml) where TDocument : class
        {
            Guard.Against<ArgumentException>(!typeof(TDocument).HasAttribute<DFeSignInfoElement>(), "Atributo [DFeSignInfoElement] não encontrado.");

            var xml = document.GetXml(options, Encoding.UTF8);
            var xmlDoc = new XmlDocument { PreserveWhitespace = true };
            xmlDoc.LoadXml(xml);

            var signatureInfo = typeof(TDocument).GetAttribute<DFeSignInfoElement>();
            var xmlSignature = GerarAssinatura(xmlDoc, signatureInfo.SignElement, signatureInfo.SignAtribute, certificado, comments, digest);

            // Adiciona a assinatura no documento e retorna o xml assinado no parametro signedXml
            var element = xmlDoc.ImportNode(xmlSignature, true);
            xmlDoc.DocumentElement.AppendChild(element);
            signedXml = xmlDoc.AsString();

            return DFeSignature.Load(xmlSignature.OuterXml);
        }

        public static bool ValidarAssinatura<TDocument>(DFeSignDocument<TDocument> document, bool gerarXml) where TDocument : class
        {
            var xml = document.Xml.IsEmpty() || gerarXml ? document.GetXml(DFeSaveOptions.DisableFormatting, Encoding.UTF8) : document.Xml;
            var xmlDoc = new XmlDocument { PreserveWhitespace = true };
            xmlDoc.LoadXml(xml);
            return ValidarAssinatura(xmlDoc);
        }

        private static XmlElement GerarAssinatura(XmlDocument doc, string infoElement, string signAtribute,
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

            var uri = infoElement.IsEmpty() || signAtribute.IsEmpty() ? "" :
                $"#{doc.GetElementsByTagName(infoElement)[0].Attributes?[signAtribute]?.InnerText}";

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
            return signedDocument.GetXml();
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
        public static bool ValidarAssinatura(XmlDocument doc)
        {
            try
            {
                var signElement = doc.GetElementsByTagName("Signature");
                Guard.Against<ACBrDFeException>(signElement.Count < 1, "Verificação falhou: Elemento [Signature] não encontrado no documento.");
                Guard.Against<ACBrDFeException>(signElement.Count > 1, "Verificação falhou: Mais de um elemento [Signature] encontrado no documento.");

                var certificateElement = doc.GetElementsByTagName("X509Certificate");
                Guard.Against<ACBrDFeException>(certificateElement.Count < 1, "Verificação falhou: Elemento [X509Certificate] não encontrado no documento.");
                Guard.Against<ACBrDFeException>(certificateElement.Count > 1, "Verificação falhou: Mais de um elemento [X509Certificate] encontrado no documento.");

                var signedXml = new SignedXml(doc);
                signedXml.LoadXml((XmlElement)signElement[0]);

                var certificate = new X509Certificate2(Convert.FromBase64String(certificateElement[0].InnerText));

                return signedXml.CheckSignature(certificate, true);
            }
            catch (Exception exception)
            {
                var log = LoggerProvider.LoggerFor(typeof(XmlSigning));
                log.Error("Erro ao validar a assinatura.", exception);
                return false;
            }
        }

        #endregion Methods
    }
}