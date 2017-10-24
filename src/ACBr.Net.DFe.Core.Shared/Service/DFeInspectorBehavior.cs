// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 07-28-2016
//
// Last Modified By : RFTD
// Last Modified On : 07-28-2016
// ***********************************************************************
// <copyright file="DFeInspectorBehavior.cs" company="ACBr.Net">
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
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace ACBr.Net.DFe.Core.Service
{
	internal class DFeInspectorBehavior : IEndpointBehavior
	{
		#region Fields

		private EventHandler<DFeMessageEventArgs> beforeSendRequest;
		private EventHandler<DFeMessageEventArgs> afterReceiveReply;

		#endregion Fields

		#region Constructors

		public DFeInspectorBehavior(EventHandler<DFeMessageEventArgs> beforeSendDFeRequest, EventHandler<DFeMessageEventArgs> afterReceiveDFeReply)
		{
			beforeSendRequest = beforeSendDFeRequest;
			afterReceiveReply = afterReceiveDFeReply;
		}

		#endregion Constructors

		#region Methods

		public void Validate(ServiceEndpoint endpoint)
		{
		}

		public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
		{
		}

		public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
		{
		}

		public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
		{
			var messageInspector = new DFeMessageInspector
			{
				BeforeSendDFeRequest = beforeSendRequest,
				AfterReceiveDFeReply = afterReceiveReply
			};
#if NETSTANDARD2_0
			clientRuntime.ClientMessageInspectors.Add(messageInspector);
#else
			clientRuntime.MessageInspectors.Add(messageInspector);
#endif
		}

		#endregion Methods
	}
}