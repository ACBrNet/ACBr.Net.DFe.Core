using System;
using System.Security.Cryptography.X509Certificates;
using ACBr.Net.DFe.Core.Attributes;
using ACBr.Net.DFe.Core.Common;

namespace ACBr.Net.DFe.Core.Document
{
    public abstract class DFeSignDocument<TDocument> : DFeDocument<TDocument> where TDocument : class
    {
        #region Properties

        [DFeElement(Ordem = int.MaxValue)]
        public DFeSignature Signature { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Assina o xml.
        /// </summary>
        /// <param name="certificado">The certificado.</param>
        /// <param name="comments">if set to <c>true</c> [comments].</param>
        /// <param name="digest">The digest.</param>
        /// <param name="options">The options.</param>
        protected void AssinarDocumento(X509Certificate2 certificado, DFeSaveOptions options, bool comments, SignDigest digest)
        {
            Signature = XmlSigning.AssinarDocumento(this, certificado, comments, digest, options, out var xml);
            Xml = xml;
        }

        #endregion Methods
    }
}