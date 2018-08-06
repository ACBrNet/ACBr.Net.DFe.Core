// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 27-03-2016
//
// Last Modified By : RFTD
// Last Modified On : 27-03-2016
// ***********************************************************************
// <copyright file="DFeSerializer.cs" company="ACBr.Net">
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
using ACBr.Net.DFe.Core.Attributes;
using ACBr.Net.DFe.Core.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ACBr.Net.DFe.Core.Serializer
{
    public class DFeSerializer
    {
        #region Constantes

        /// <summary>
        /// The er r_ ms g_ maior
        /// </summary>
        internal const string ErrMsgMaior = "Tamanho maior que o máximo permitido";

        /// <summary>
        /// The er r_ ms g_ menor
        /// </summary>
        internal const string ErrMsgMenor = "Tamanho menor que o mínimo permitido";

        /// <summary>
        /// The er r_ ms g_ vazio
        /// </summary>
        internal const string ErrMsgVazio = "Nenhum valor informado";

        /// <summary>
        /// The er r_ ms g_ invalido
        /// </summary>
        internal const string ErrMsgInvalido = "Conteúdo inválido";

        /// <summary>
        /// The er r_ ms g_ maxim o_ decimais
        /// </summary>
        internal const string ErrMsgMaximoDecimais = "Numero máximo de casas decimais permitidas";

        /// <summary>
        /// The er r_ ms g_ maio r_ maximo
        /// </summary>
        internal const string ErrMsgMaiorMaximo = "Número de ocorrências maior que o máximo permitido - Máximo ";

        /// <summary>
        /// The er r_ ms g_ fina l_ meno r_ inicial
        /// </summary>
        internal const string ErrMsgFinalMenorInicial = "O numero final não pode ser menor que o inicial";

        /// <summary>
        /// The er r_ ms g_ arquiv o_ na o_ encontrado
        /// </summary>
        internal const string ErrMsgArquivoNaoEncontrado = "Arquivo não encontrado";

        /// <summary>
        /// The er r_ ms g_ soment e_ um
        /// </summary>
        internal const string ErrMsgSomenteUm = "Somente um campo deve ser preenchido";

        /// <summary>
        /// The er r_ ms g_ meno r_ minimo
        /// </summary>
        internal const string ErrMsgMenorMinimo = "Número de ocorrências menor que o mínimo permitido - Mínimo ";

        /// <summary>
        /// The ds c_ CNPJ
        /// </summary>
        internal const string DscCnpj = "CNPJ(MF)";

        /// <summary>
        /// The ds c_ CPF
        /// </summary>
        internal const string DscCpf = "CPF";

        #endregion Constantes

        #region Fields

        private readonly Type tipoDFe;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DFeSerializer{T}"/> class.
        /// </summary>
        internal DFeSerializer(Type tipo)
        {
            Guard.Against<ArgumentException>(tipo.IsGenericType, "Não é possivel serializar uma classe generica !");
            Guard.Against<ArgumentException>(!tipo.HasAttribute<DFeRootAttribute>(), "Não é uma classe DFe !");

            tipoDFe = tipo;
            Options = new SerializerOptions();
        }

        #endregion Constructors

        #region Propriedades

        /// <summary>
        /// Gets the options.
        /// </summary>
        /// <value>The options.</value>
        public SerializerOptions Options { get; }

        #endregion Propriedades

        #region Methods

        #region Create

        /// <summary>
        /// Creates the serializer.
        /// </summary>
        /// <param name="tipo">The tipo.</param>
        /// <returns>ACBr.Net.DFe.Core.Serializer.DFeSerializer.</returns>
        public static DFeSerializer CreateSerializer(Type tipo)
        {
            return new DFeSerializer(tipo);
        }

        /// <summary>
        /// Creates the serializer.
        /// </summary>
        /// <typeparam name="TCreate"></typeparam>
        /// <returns>DFeSerializer.</returns>
        public static DFeSerializer<TCreate> CreateSerializer<TCreate>() where TCreate : class
        {
            return new DFeSerializer<TCreate>();
        }

        #endregion Create

        #region Serialize

        /// <summary>
        /// Serializes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="path">The xml.</param>
        public bool Serialize(object item, string path)
        {
            Guard.Against<ArgumentException>(item.GetType() != tipoDFe, "Tipo diferente do informado");

            Options.ErrosAlertas.Clear();

            if (item.IsNull())
            {
                Options.ErrosAlertas.Add("O item é nulo !");
                return false;
            }

            var xmldoc = Serialize(item);
            var ret = !Options.ErrosAlertas.Any();
            var xml = xmldoc.AsString(Options.FormatarXml, !Options.OmitirDeclaracao, Options.Encoding);
            File.WriteAllText(path, xml, Options.Encoding);

            return ret;
        }

        /// <summary>
        /// Serializes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="stream">The stream.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Serialize(object item, Stream stream)
        {
            Guard.Against<ArgumentException>(item.GetType() != tipoDFe, "Tipo diferente do informado");

            Options.ErrosAlertas.Clear();
            if (item.IsNull())
            {
                Options.ErrosAlertas.Add("O item é nulo !");
                return false;
            }

            var xmldoc = Serialize(item);
            var ret = !Options.ErrosAlertas.Any();
            var xml = xmldoc.AsString(Options.FormatarXml, !Options.OmitirDeclaracao, Options.Encoding);

            using (var ms = new MemoryStream())
            using (var sw = new StreamWriter(ms, Options.Encoding))
            {
                sw.WriteLine(xml);
                sw.Flush();
                ms.WriteTo(stream);
            }

            stream.Position = 0;
            return ret;
        }

        private XDocument Serialize(object item)
        {
            var xmldoc = Options.OmitirDeclaracao ? new XDocument() : new XDocument(new XDeclaration("1.0", "UTF-8", null));

            var rooTag = tipoDFe.GetAttribute<DFeRootAttribute>();

            var rootName = rooTag.Name;

            if (rootName.IsEmpty())
            {
                var root = tipoDFe.GetRootName(item);
                rootName = root.IsEmpty() ? tipoDFe.Name : root;
            }

            var rootElement = ObjectSerializer.Serialize(item, tipoDFe, rootName, rooTag.Namespace, Options);
            xmldoc.Add(rootElement);
            xmldoc.RemoveEmptyNs();

            return xmldoc;
        }

        #endregion Serialize

        #region Deserialize

        /// <summary>
        /// Deserializes the specified xml.
        /// </summary>
        /// <param name="xml">The xml.</param>
        /// <returns>System.Object.</returns>
        public object Deserialize(string xml)
        {
            var content = File.Exists(xml) ? File.ReadAllText(xml, Options.Encoding) : xml;
            var xmlDoc = XDocument.Parse(content);
            return Deserialize(xmlDoc);
        }

        /// <summary>
        /// Deserializes the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>System.Object.</returns>
        public object Deserialize(Stream stream)
        {
            using (var reader = new StreamReader(stream, Options.Encoding))
            {
                stream.Position = 0;
                var xmlDoc = XDocument.Parse(reader.ReadToEnd());
                return Deserialize(xmlDoc);
            }
        }

        private object Deserialize(XDocument xmlDoc)
        {
            Options.ErrosAlertas.Clear();

            var rootTag = tipoDFe.GetAttribute<DFeRootAttribute>();

            var rootNames = new List<string>();
            if (!rootTag.Name.IsEmpty())
            {
                rootNames.Add(rootTag.Name);
                rootNames.Add(tipoDFe.Name);
            }
            else
            {
                rootNames.AddRange(tipoDFe.GetRootNames());
                rootNames.Add(tipoDFe.Name);
            }

            var xmlNode = (from node in xmlDoc.Descendants()
                           where node.Name.LocalName.IsIn(rootNames)
                           select node).FirstOrDefault();

            Guard.Against<ACBrDFeException>(xmlNode == null, "Nenhum objeto root encontrado !");

            var returnValue = ObjectSerializer.Deserialize(tipoDFe, xmlNode, Options);

            return returnValue;
        }

        #endregion Deserialize

        #endregion Methods
    }
}