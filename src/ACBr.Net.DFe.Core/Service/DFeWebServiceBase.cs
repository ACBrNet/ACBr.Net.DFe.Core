// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 07-28-2016
//
// Last Modified By : RFTD
// Last Modified On : 07-28-2016
// ***********************************************************************
// <copyright file="DFeWebserviceBase.cs" company="ACBr.Net">
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

using ACBr.Net.Core.Logging;
using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;

namespace ACBr.Net.DFe.Core.Service
{
	/// <summary>
	/// Classe base para comunição com websrvices.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class DFeWebserviceBase<T> : ClientBase<T>, IACBrLog where T : class
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="url"></param>
		/// <param name="timeOut"></param>
		/// <param name="certificado"></param>
		protected DFeWebserviceBase(string url, TimeSpan? timeOut = null, X509Certificate2 certificado = null) : base(new BasicHttpBinding(), new EndpointAddress(url))
		{
			((BasicHttpBinding)Endpoint.Binding).UseDefaultWebProxy = true;

			if (ClientCredentials != null)
			{
				ClientCredentials.ClientCertificate.Certificate = certificado;
				if (certificado != null)
				{
					((BasicHttpBinding)Endpoint.Binding).Security.Mode = BasicHttpSecurityMode.Transport;
					((BasicHttpBinding)Endpoint.Binding).Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;
				}
			}

			var endpointInspector = new DFeInspectorBehavior();
			Endpoint.Behaviors.Add(endpointInspector);

			if (!timeOut.HasValue)
				return;

			Endpoint.Binding.OpenTimeout = timeOut.Value;
			Endpoint.Binding.ReceiveTimeout = timeOut.Value;
			Endpoint.Binding.SendTimeout = timeOut.Value;
		}
	}
}