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

using ACBr.Net.Core.Logging;
using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using ACBr.Net.Core.Extensions;

namespace ACBr.Net.DFe.Core.Service
{
    /// <summary>
    /// Class DFeServiceClientBase.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="ClientBase{T}" />
    /// <seealso cref="IACBrLog" />

    public abstract class DFeServiceClientBase<T> : ClientBase<T>, IACBrLog, IDisposable where T : class
    {
        #region Constructors

        /// <summary>
        /// Inicializa uma nova instancia da classe <see cref="DFeServiceClientBase{T}"/>.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="timeOut"></param>
        /// <param name="certificado"></param>
        protected DFeServiceClientBase(string url, TimeSpan? timeOut = null, X509Certificate2 certificado = null) : base(new BasicHttpBinding(), new EndpointAddress(url))
        {
            if (!(Endpoint?.Binding is BasicHttpBinding binding)) return;

            binding.UseDefaultWebProxy = true;

            if (ClientCredentials != null)
                ClientCredentials.ClientCertificate.Certificate = certificado;

            if (url.Trim().ToLower().StartsWith("https"))
                binding.Security.Mode = BasicHttpSecurityMode.Transport;

            if (certificado != null)
                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;

            var endpointInspector = new DFeInspectorBehavior((sender, args) => BeforeSendDFeRequest(args.Message),
                                                             (sender, args) => AfterReceiveDFeReply(args.Message));
#if NETSTANDARD2_0
			Endpoint.EndpointBehaviors.Add(endpointInspector);
#else
            Endpoint.Behaviors.Add(endpointInspector);
#endif

            if (!timeOut.HasValue) return;

            binding.OpenTimeout = timeOut.Value;
            binding.ReceiveTimeout = timeOut.Value;
            binding.SendTimeout = timeOut.Value;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        protected virtual void BeforeSendDFeRequest(string message)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        protected virtual void AfterReceiveDFeReply(string message)
        {
        }

        #endregion Methods

        #region IDisposable

        /// <inheritdoc />
        public void Dispose()
        {
            if (State == CommunicationState.Faulted)
                Abort();

            if (State.IsIn(CommunicationState.Closed, CommunicationState.Closing)) return;

            Close();
        }

        #endregion IDisposable
    }
}