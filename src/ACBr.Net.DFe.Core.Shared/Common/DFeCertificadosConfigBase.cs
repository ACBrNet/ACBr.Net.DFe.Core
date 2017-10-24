// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 01-31-2016
//
// Last Modified By : RFTD
// Last Modified On : 06-07-2016
// ***********************************************************************
// <copyright file="DFeCertificados.cs" company="ACBr.Net">
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

using ACBr.Net.Core.Extensions;
using System;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using ACBr.Net.DFe.Core.Extensions;

namespace ACBr.Net.DFe.Core.Common
{
	/// <summary>
	/// Class NFECFGCertificados. This class cannot be inherited.
	/// </summary>
	public abstract class DFeCertificadosConfigBase
	{
		#region Fields

		private DateTime dataVenc;
		private string subjectName;
		private string cnpj;

		#endregion Fields

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="DFeCertificadosConfigBase"/> class.
		/// </summary>
		protected DFeCertificadosConfigBase()
		{
			dataVenc = DateTime.MinValue;
			Certificado = string.Empty;
			subjectName = string.Empty;
			cnpj = string.Empty;
		}

		#endregion Constructor

		#region Properties

		/// <summary>
		/// Define/retorna o certificado ou Numero de Serie.
		/// </summary>
		/// <value>O Certificado/Numero de Serie.</value>
		[Browsable(true)]
		public string Certificado { get; set; }

		/// <summary>
		/// Define/retorna o certificado em bytes.
		/// </summary>
		/// <value>O Certificado.</value>
		[Browsable(true)]
		public byte[] CertificadoBytes { get; set; }

		/// <summary>
		/// Define/retorna a senha do certificado.
		/// </summary>
		/// <value>A senha.</value>
		[Browsable(true)]
		public string Senha { get; set; }

		/// <summary>
		/// Retorna a data de vencimento do certificado.
		/// </summary>
		/// <value>A data de vencimento.</value>
		public DateTime DataVenc
		{
			get
			{
				if (dataVenc == DateTime.MinValue && !Certificado.IsEmpty() && !CertificadoBytes.IsNullOrEmpty())
					GetCertificado();

				return dataVenc;
			}
		}

		/// <summary>
		/// Define/retorna o nome do responsável pelo certificado.
		/// </summary>
		/// <value>The name of the subject.</value>
		public string Nome
		{
			get
			{
				if (subjectName.IsEmpty() && !Certificado.IsEmpty() && !CertificadoBytes.IsNullOrEmpty())
					GetCertificado();

				return subjectName;
			}
		}

		/// <summary>
		/// Gets or sets the CNPJ.
		/// </summary>
		/// <value>The CNPJ.</value>
		public string CNPJ
		{
			get
			{
				if (cnpj.IsEmpty() && !Certificado.IsEmpty() && !CertificadoBytes.IsNullOrEmpty())
					GetCertificado();

				return cnpj;
			}
		}

		#endregion Properties

		#region Methods

		/// <summary>
		/// Seleciona um certificado digital instalado na maquina retornando o numero de serie do mesmo.
		/// </summary>
		/// <returns>Numero de Serie.</returns>
		public string SelecionarCertificado()
		{
			var cert = CertificadoDigital.SelecionarCertificado(string.Empty);
			return cert.GetSerialNumberString();
		}

		/// <summary>
		/// retorna o certificado digital de acordo com os dados informados.
		/// </summary>
		/// <returns>X509Certificate2.</returns>
		public X509Certificate2 ObterCertificado()
		{
			if (CertificadoBytes?.Length > 0)
			{
				return CertificadoDigital.SelecionarCertificado(CertificadoBytes, Senha);
			}

			if (File.Exists(Certificado))
			{
				return CertificadoDigital.SelecionarCertificado(Certificado, Senha);
			}

			var ret = CertificadoDigital.SelecionarCertificado(Certificado);
			if (!Senha.IsEmpty())
			{
				ret.SetPin(Senha);
			}

			return ret;
		}

		/// <summary>
		/// Gets the certificado.
		/// </summary>
		protected void GetCertificado()
		{
			var cert = ObterCertificado();
			dataVenc = cert.GetExpirationDateString().ToData();
			subjectName = cert.SubjectName.Name;
			cnpj = cert.GetCNPJ();
		}

		#endregion Methods
	}
}