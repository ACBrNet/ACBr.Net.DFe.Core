// ***********************************************************************
// Assembly         : ACBr.Net.NFe
// Author           : RFTD
// Created          : 07-26-2014
//
// Last Modified By : RFTD
// Last Modified On : 10-08-2014
// ***********************************************************************
// <copyright file="GenericNFeCollection.cs" company="ACBr.Net">
//     Copyright (c) ACBr.Net. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using ACBr.Net.Core.Generics;

namespace ACBr.Net.DFe.Core.Collection
{
    /// <summary>
    /// Classe GenericNFeCollection.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DFeCollection<T> : GenericCollection<T> where T : class
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
		public DFeCollection(IEnumerable<T> source)
	    {
		    List = source.ToList();
	    }

		#endregion Constructors

		#region Methods

		/// <summary>
		/// Adiciona novo item na coleção e retorna  item criado
		/// </summary>
		/// <returns>T.</returns>
		public virtual T AddNew()
        {
            var item = (T)Activator.CreateInstance(typeof (T), true);
            List.Add(item);
            return item;
        }

		/// <summary>
		/// Adds the range.
		/// </summary>
		/// <param name="item">The item.</param>
		public virtual void Add(T item)
		{
			List.Add(item);
		}

		/// <summary>
		/// Adds the range.
		/// </summary>
		/// <param name="itens">The itens.</param>
		public virtual void AddRange(IEnumerable<T> itens)
	    {
		    List.AddRange(itens);
	    }

		#endregion Methods

		#region Operators

		/// <summary>
		/// Performs an implicit conversion from <see cref="List{T}"/> to <see cref="DFeCollection{T}"/>.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns>The result of the conversion.</returns>
		public static implicit operator DFeCollection<T>(List<T> source)
	    {
		    return new DFeCollection<T>(source);
	    }

		/// <summary>
		/// Performs an implicit conversion from <see cref="DFeCollection{T}"/> to <see cref="List{T}"/>.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns>The result of the conversion.</returns>
		public static implicit operator List<T>(DFeCollection<T> source)
		{
			return source.List;
		}

		#endregion Operators
	}
}