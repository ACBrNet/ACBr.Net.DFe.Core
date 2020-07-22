// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 27-03-2016
//
// Last Modified By : RFTD
// Last Modified On : 27-03-2016
// ***********************************************************************
// <copyright file="DFeSerializer.Extendend.cs" company="ACBr.Net">
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

using System.IO;

namespace ACBr.Net.DFe.Core.Serializer
{
    /// <summary>
    /// Class DFeSerializer. This class cannot be inherited.
    /// </summary>
    /// <typeparam name="TClass"></typeparam>
    /// <seealso>
    ///     <cref>ACBr.Net.DFe.Core.Serializer.DFeSerializerBase</cref>
    /// </seealso>
    public sealed class DFeSerializer<TClass> : DFeSerializer where TClass : class
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DFeSerializer{TClass}"/> class.
        /// </summary>
        internal DFeSerializer() : base(typeof(TClass))
        {
        }

        #endregion Constructors

        #region Create

        /// <summary>
        /// Creates the serializer.
        /// </summary>
        /// <typeparam name="TCreate"></typeparam>
        /// <returns>DFeSerializer.</returns>
        public static DFeSerializer<TCreate> CreateSerializer<TCreate>() where TCreate : class
        {
            return new DFeSerializer<TCreate>();
        }

        #endregion Create

        #region Methods

        #region Serialize

        /// <summary>
        /// Serializes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="path">The xml.</param>
        public bool Serialize(TClass item, string path)
        {
            return base.Serialize(item, path);
        }

        /// <summary>
        /// Serializes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="stream">The stream.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Serialize(TClass item, Stream stream)
        {
            return base.Serialize(item, stream);
        }

        #endregion Serialize

        #region Deserialize

        /// <summary>
        /// Deserializes the specified xml.
        /// </summary>
        /// <param name="path">The xml.</param>
        /// <returns>T.</returns>
        public new TClass Deserialize(string path)
        {
            return (TClass)base.Deserialize(path);
        }

        /// <summary>
        /// Deserializes the specified xml.
        /// </summary>
        /// <param name="stream">The xml.</param>
        /// <returns>T.</returns>
        public new TClass Deserialize(Stream stream)
        {
            return (TClass)base.Deserialize(stream);
        }

        #endregion Deserialize

        #endregion Methods
    }
}