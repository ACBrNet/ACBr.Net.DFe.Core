// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 06-11-2017
//
// Last Modified By : RFTD
// Last Modified On : 06-11-2017
// ***********************************************************************
// <copyright file="DFeParentCollection.cs" company="ACBr.Net">
//		        		   The MIT License (MIT)
//	     		    Copyright (c) 2014 - 2017 Grupo ACBr.Net
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
using ACBr.Net.DFe.Core.Attributes;

namespace ACBr.Net.DFe.Core.Collection
{
    /// <inheritdoc />
    public class DFeParentCollection<TTipo, TParent> : DFeCollection<TTipo>
        where TParent : class
        where TTipo : DFeParentItem<TTipo, TParent>
    {
        #region Fields

        protected TParent parent;

        #endregion Fields

        #region Propriedades

        /// <summary>
        /// Gets the parent.
        /// </summary>
        /// <value>The parent.</value>
        [DFeIgnore]
        public TParent Parent
        {
            get => parent;
            set
            {
                parent = value;
                foreach (var item in this)
                {
                    if (item.Parent == value) continue;

                    item.Parent = value;
                }
            }
        }

        #endregion Propriedades

        #region Methods

        /// <summary>
        /// Adds an object to the end of the <see cref="DFeCollection{T}"/>.
        /// </summary>
        /// <returns>T.</returns>
        public override TTipo AddNew()
        {
            var item = (TTipo)Activator.CreateInstance(typeof(TTipo), true);
            item.Parent = Parent;
            base.Add(item);
            return item;
        }

        /// <summary>Adds an object to the end of the <see cref="DFeCollection{T}"/>.</summary>
        /// <param name="item">The object to be added to the end of the <see cref="DFeCollection{T}"/>. The value can be null for reference types.</param>
        public override void Add(TTipo item)
        {
            item.Parent = Parent;
            base.Add(item);
        }

        /// <summary>Inserts an element into the <see cref="DFeCollection{T}"/> at the specified index.</summary>
        /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
        /// <param name="item">The object to insert. The value can be null for reference types.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="index" /> is less than 0.-or-<paramref name="index" /> is greater than <see cref="DFeCollection{T}.Count"/>.</exception>
        public override void Insert(int index, TTipo item)
        {
            item.Parent = Parent;
            base.Insert(index, item);
        }

        /// <summary>Inserts the elements of a collection into the <see cref="DFeCollection{T}"/> at the specified index.</summary>
        /// <param name="index">The zero-based index at which the new elements should be inserted.</param>
        /// <param name="collection">The collection whose elements should be inserted into the <see cref="DFeCollection{T}"/>. The collection itself cannot be null, but it can contain elements that are null, if type <paramref name="T" /> is a reference type.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="collection" /> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="index" /> is less than 0.-or-<paramref name="index" /> is greater than <see cref="DFeCollection{T}.Count"/>.</exception>
        public override void InsertRange(int index, IEnumerable<TTipo> collection)
        {
            foreach (var item in collection)
            {
                item.Parent = Parent;
            }

            base.InsertRange(index, collection);
        }

        #endregion Methods
    }
}