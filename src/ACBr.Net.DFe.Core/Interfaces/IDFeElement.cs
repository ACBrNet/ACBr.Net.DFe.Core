// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 05-04-2016
//
// Last Modified By : RFTD
// Last Modified On : 05-04-2016
// ***********************************************************************
// <copyright file="IDFeElement.cs" company="ACBr.Net">
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
using ACBr.Net.DFe.Core.Serializer;

namespace ACBr.Net.DFe.Core.Interfaces
{
	/// <summary>
	/// Interface IDFeElement
	/// </summary>
	public interface IDFeElement
	{
		/// <summary>
		/// Gets or sets the descricao.
		/// </summary>
		/// <value>The descricao.</value>
		string Descricao { get; set; }
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		string Id { get; set; }
		/// <summary>
		/// Gets or sets the maximum.
		/// </summary>
		/// <value>The maximum.</value>
		int Max { get; set; }
		/// <summary>
		/// Gets or sets the minimum.
		/// </summary>
		/// <value>The minimum.</value>
		int Min { get; set; }
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		string Name { get; set; }
		/// <summary>
		/// Gets or sets the ocorrencias.
		/// </summary>
		/// <value>The ocorrencias.</value>
		int Ocorrencias { get; set; }
		/// <summary>
		/// Gets or sets the tipo.
		/// </summary>
		/// <value>The tipo.</value>
		TipoCampo Tipo { get; set; }
	}
}