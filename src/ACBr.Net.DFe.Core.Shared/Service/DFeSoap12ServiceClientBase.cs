// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 07-28-2016
//
// Last Modified By : RFTD
// Last Modified On : 07-28-2016
// ***********************************************************************
// <copyright file="DFeServiceClientBase.cs" company="ACBr.Net">
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
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Channels;

namespace ACBr.Net.DFe.Core.Service
{
	/// <summary>
	/// Class DFeSoap12ServiceClientBase.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <seealso cref="ACBr.Net.DFe.Core.Service.DFeServiceClientBase{T}" />
	public abstract class DFeSoap12ServiceClientBase<T> : DFeServiceClientBase<T> where T : class
	{
		/// <summary>
		/// Inicializa uma nova instancia da classe <see cref="DFeSoap12ServiceClientBase{T}"/>.
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <param name="timeOut">The time out.</param>
		/// <param name="certificado">The certificado.</param>
		protected DFeSoap12ServiceClientBase(string url, TimeSpan? timeOut = null, X509Certificate2 certificado = null) : base(url, timeOut, certificado)
		{
			var custom = new CustomBinding(Endpoint.Binding);
			var version = custom.Elements.Find<TextMessageEncodingBindingElement>();
			version.MessageVersion = MessageVersion.Soap12WSAddressing10;

			Endpoint.Binding = custom;
		}
	}
}