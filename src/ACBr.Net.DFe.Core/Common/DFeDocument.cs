// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 09-11-2016
//
// Last Modified By : RFTD
// Last Modified On : 09-11-2016
// ***********************************************************************
// <copyright file="DFeDocument.cs" company="ACBr.Net">
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

using ACBr.Net.Core.Generics;
using ACBr.Net.DFe.Core.Serializer;
using System.IO;
using System.Text;

#region COM Interop Attributes

#if COM_INTEROP

using System.Runtime.InteropServices;

#endif

#endregion COM Interop Attributes

namespace ACBr.Net.DFe.Core.Common
{
	#region COM Interop Attributes

#if COM_INTEROP

	[ComVisible(false)]
#endif

	#endregion COM Interop Attributes

	public abstract class DFeDocument<TDocument> : GenericClone<TDocument> where TDocument : class
	{
		/// <summary>
		/// Carrega o documento.
		/// </summary>
		/// <param name="document">The document.</param>
		/// <returns>TDocument.</returns>
		public static TDocument Load(string document)
		{
			var serializer = new DFeSerializer(typeof(TDocument));
			return (TDocument)serializer.Deserialize(document);
		}

		/// <summary>
		/// Carrega o documento.
		/// </summary>
		/// <param name="document">The document.</param>
		/// <param name="encoding"></param>
		/// <returns>TDocument.</returns>
		public static TDocument Load(Stream document, Encoding encoding = null)
		{
			var serializer = new DFeSerializer(typeof(TDocument));
			if (encoding != null)
				serializer.Options.Encoder = encoding;

			return (TDocument)serializer.Deserialize(document);
		}

		/// <summary>
		/// Retorna o Xml do documento.
		/// </summary>
		/// <param name="options">The options.</param>
		/// <returns>System.String.</returns>
		public string GetXml(DFeSaveOptions options = DFeSaveOptions.None)
		{
			using (var stream = new MemoryStream())
			{
				Save(stream, options);
				stream.Position = 0;
				using (var streamReader = new StreamReader(stream))
				{
					return streamReader.ReadToEnd();
				}
			}
		}

		/// <summary>
		/// Salva o documento.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <param name="options">The options.</param>
		/// <returns>TDocument.</returns>
		public void Save(string path, DFeSaveOptions options = DFeSaveOptions.None)
		{
			var serializer = new DFeSerializer(typeof(TDocument));

			if (!options.HasFlag(DFeSaveOptions.None))
			{
				serializer.Options.RemoverAcentos = options.HasFlag(DFeSaveOptions.RemoveAccents);
				serializer.Options.FormatarXml = !options.HasFlag(DFeSaveOptions.DisableFormatting);
				serializer.Options.OmitirDeclaracao = !options.HasFlag(DFeSaveOptions.OmitDeclaration);
			}

			serializer.Serialize(this, path);
		}

		/// <summary>
		/// Salva o documento.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <param name="options">The options.</param>
		/// <returns>TDocument.</returns>
		public void Save(Stream stream, DFeSaveOptions options = DFeSaveOptions.None)
		{
			var serializer = new DFeSerializer(typeof(TDocument));

			if (!options.HasFlag(DFeSaveOptions.None))
			{
				serializer.Options.RemoverAcentos = options.HasFlag(DFeSaveOptions.RemoveAccents);
				serializer.Options.FormatarXml = !options.HasFlag(DFeSaveOptions.DisableFormatting);
				serializer.Options.OmitirDeclaracao = !options.HasFlag(DFeSaveOptions.OmitDeclaration);
			}

			serializer.Serialize(this, stream);
		}
	}
}