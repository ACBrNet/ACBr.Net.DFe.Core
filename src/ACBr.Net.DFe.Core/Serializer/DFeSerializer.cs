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
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ACBr.Net.DFe.Core.Serializer
{
	/// <summary>
	/// Class DFeSerializer.
	/// </summary>
	public class DFeSerializer
	{
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
		/// <typeparam name="T"></typeparam>
		/// <returns>DFeSerializer.</returns>
		public static DFeSerializer<T> CreateSerializer<T>() where T : class
		{
			return new DFeSerializer<T>();
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
			if (Options.FormatarXml)
				xmldoc.Save(path);
			else
				xmldoc.Save(path, SaveOptions.DisableFormatting);
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
			if (Options.FormatarXml)
				xmldoc.Save(stream);
			else
				xmldoc.Save(stream, SaveOptions.DisableFormatting);

			stream.Position = 0;
			return ret;
		}

		private XDocument Serialize(object item)
		{
			var xmldoc = Options.OmitirDeclaracao ? new XDocument() : new XDocument(new XDeclaration("1.0", "UTF-8", null));

			var rooTag = item.GetType().GetAttribute<DFeRootAttribute>();
			var rootName = rooTag != null && !rooTag.Name.IsEmpty()
				? rooTag.Name : tipoDFe.Name;

			var rootElement = ObjectSerializer.Serialize(item, tipoDFe, rootName, Options);
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
			var content = File.Exists(xml) ? File.ReadAllText(xml, Options.Encoder) : xml;
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
			using (var reader = new StreamReader(stream, Options.Encoder))
			{
				var xmlDoc = XDocument.Parse(reader.ReadToEnd());
				return Deserialize(xmlDoc);
			}
		}

		private object Deserialize(XDocument xmlDoc)
		{
			var rootTag = tipoDFe.GetAttribute<DFeRootAttribute>();
			var rootName = rootTag != null && !rootTag.Name.IsEmpty()
								? rootTag.Name : tipoDFe.Name;

			var xmlNode = xmlDoc.Root;

			if (xmlDoc.Root?.Name != rootName)
			{
				xmlNode = (from node in xmlDoc.Descendants()
						   where node.Name.LocalName == rootName
						   select node).FirstOrDefault();
			}

			Guard.Against<ACBrDFeException>(xmlNode == null, $"Nenhum objeto {rootName} encontrado !");

			return ObjectSerializer.Deserialize(tipoDFe, xmlNode, Options);
		}

		#endregion Deserialize

		#endregion Methods
	}
}