// ***********************************************************************
// Assembly         : ACBr.Net.NFe
// Author           : RFTD
// Created          : 07-26-2014
//
// Last Modified By : RFTD
// Last Modified On : 10-21-2016
// ***********************************************************************
// <copyright file="GenericNFeCollection.cs" company="ACBr.Net">
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
using System.Collections.Generic;

namespace ACBr.Net.DFe.Core.Collection
{
	/// <summary>
	/// Classe GenericNFeCollection.
	/// </summary>
	/// <typeparam name="TTipo"></typeparam>
	[Serializable]
	public class DFeCollection<TTipo> : List<TTipo>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="DFeCollection{T}"/> class.
		/// </summary>
		public DFeCollection()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DFeCollection{T}"/> class.
		/// </summary>
		/// <param name="source">The source.</param>
		public DFeCollection(IEnumerable<TTipo> source)
		{
			AddRange(source);
		}

		#endregion Constructors

		#region Methods

		/// <summary>
		/// Adiciona novo item na coleção e retorna  item criado
		/// </summary>
		/// <returns>T.</returns>
		public virtual TTipo AddNew()
		{
			var item = (TTipo)Activator.CreateInstance(typeof(TTipo), true);
			base.Add(item);
			return item;
		}

		/// <summary>
		/// Adds the range.
		/// </summary>
		/// <param name="item">The item.</param>
		public new virtual void Add(TTipo item)
		{
			base.Add(item);
		}

		/// <summary>
		/// Adds the range.
		/// </summary>
		/// <param name="itens">The itens.</param>
		public new virtual void AddRange(IEnumerable<TTipo> itens)
		{
			base.AddRange(itens);
		}

		#endregion Methods

		#region Operators

		/// <summary>
		/// Performs an implicit conversion from <see cref="TTipo[]"/> to <see cref="DFeCollection{TTipo}"/>.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns>The result of the conversion.</returns>
		public static implicit operator DFeCollection<TTipo>(TTipo[] source)
		{
			return new DFeCollection<TTipo>(source);
		}

		#endregion Operators
	}
}