// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 03-10-2018
//
// Last Modified By : RFTD
// Last Modified On : 03-10-2018
// ***********************************************************************
// <copyright file="EnumExtensions.cs" company="ACBr.Net">
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
using System.Linq;
using ACBr.Net.DFe.Core.Attributes;
using ACBr.Net.DFe.Core.Common;

namespace ACBr.Net.DFe.Core.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Retorna o valor do Enum definido pelo DFeEnumAttribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public static string GetDFeValue<T>(this T value) where T : Enum
        {
            var member = typeof(T).GetMember(value.ToString()).FirstOrDefault();
            var enumAttribute = member?.GetCustomAttributes(false).OfType<DFeEnumAttribute>().FirstOrDefault();
            var enumValue = enumAttribute?.Value;
            return enumValue ?? value.ToString();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="uf"></param>
        /// <returns></returns>
        public static DFeCodUF ToCodeUf(this DFeSiglaUF uf)
        {
            return (DFeCodUF)Enum.Parse(typeof(DFeCodUF), uf.ToString());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="uf"></param>
        /// <returns></returns>
        public static DFeSiglaUF ToSiglaUF(this DFeCodUF uf)
        {
            return (DFeSiglaUF)Enum.Parse(typeof(DFeSiglaUF), uf.ToString());
        }
    }
}