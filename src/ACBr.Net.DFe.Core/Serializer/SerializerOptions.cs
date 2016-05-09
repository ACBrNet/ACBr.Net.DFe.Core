// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 05-04-2016
//
// Last Modified By : RFTD
// Last Modified On : 05-04-2016
// ***********************************************************************
// <copyright file="SerializerOptions.cs" company="ACBr.Net">
//     Copyright © ACBr.Net 2014 - 2016
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.ComponentModel;

namespace ACBr.Net.DFe.Core.Serializer
{
	/// <summary>
	/// Class SerializerOptions. This class cannot be inherited.
	/// </summary>
	public class SerializerOptions
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

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="DFeSerializer" /> class.
		/// </summary>
		internal SerializerOptions()
		{
			ErrosAlertas = new BindingList<string>();
			FormatoAlerta = "TAG:%TAG% ID:%ID%/%TAG%(%DESCRICAO%) - %MSG%.";
			RetirarAcentos = false;
			IdentarXml = true;
		}

		#endregion Constructors

		#region Propriedades

		/// <summary>
		/// Gets or sets a value indicating whether [retirar acentos].
		/// </summary>
		/// <value><c>true</c> if [retirar acentos]; otherwise, <c>false</c>.</value>
		public bool RetirarAcentos { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SerializerOptions"/> is identar.
		/// </summary>
		/// <value><c>true</c> if identar; otherwise, <c>false</c>.</value>
		public bool IdentarXml { get; set; }

		/// <summary>
		/// Gets the lista de alertas.
		/// </summary>
		/// <value>The lista de alertas.</value>
		public BindingList<string> ErrosAlertas { get; }

		/// <summary>
		/// Gets or sets the formato alerta.
		/// </summary>
		/// <value>The formato alerta.</value>
		public string FormatoAlerta { get; set; }

		#endregion Propriedades

		#region Methods

		/// <summary>
		/// Ws the alerta.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="tag">The tag.</param>
		/// <param name="descricao">The descricao.</param>
		/// <param name="alerta">The alerta.</param>
		internal void WAlerta(string id, string tag, string descricao, string alerta)
		{
			// O Formato da mensagem de erro pode ser alterado pelo usuario alterando-se a property FormatoAlerta: onde;
			// %TAG%       : Representa a TAG; ex: <nLacre>
			// %ID%        : Representa a ID da TAG; ex X34
			// %MSG%       : Representa a mensagem de alerta
			// %DESCRICAO% : Representa a Descrição da TAG

			var s = FormatoAlerta.Clone() as string;
			if (s == null)
				return;

			s = s.Replace("%ID%", id).Replace("%TAG%", $"<{tag}>").Replace("%DESCRICAO%", descricao).Replace("%MSG%", alerta);

			ErrosAlertas.Add(s);
		}
		
		#endregion Methods
	}
}