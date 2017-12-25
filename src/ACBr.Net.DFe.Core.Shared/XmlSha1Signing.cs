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
    public static class XmlSha1Signing
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
        /// <returns>System.String.</returns>
        /// <exception cref="ACBrDFeException">Erro ao efetuar assinatura digital.</exception>
        /// <exception cref="ACBrDFeException">Erro ao efetuar assinatura digital.</exception>
        public static string AssinarXml(string xml, string docElement, string infoElement, X509Certificate2 pCertificado, bool comments = false)
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
        /// <returns>System.String.</returns>
        /// <exception cref="ACBrDFeException">Erro ao efetuar assinatura digital.</exception>
        /// <exception cref="ACBrDFeException">Erro ao efetuar assinatura digital.</exception>
        public static string AssinarXmlTodos(string xml, string docElement, string infoElement, X509Certificate2 certificado, bool comments = false)
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

        private static void AssinarDocumento(XmlDocument doc, string docElement, string infoElement, X509Certificate2 certificado, bool comments = false)
        {
            Guard.Against<ArgumentException>(!infoElement.IsEmpty() && doc.GetElementsByTagName(infoElement).Count != 1, "Referencia invalida ou não é unica.");

            //Adiciona Certificado ao Key Info
            var keyInfo = new KeyInfo();
            keyInfo.AddClause(new KeyInfoX509Data(certificado));

            //Seta chaves
            var signedDocument = new SignedXml(doc)
            {
                SigningKey = certificado.PrivateKey,
                KeyInfo = keyInfo
            };

            var uri = infoElement.IsEmpty() ? "" : $"#{doc.GetElementsByTagName(infoElement)[0].Attributes?["Id"]?.InnerText}";

            // Cria referencia
            var reference = new Reference { Uri = uri };

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