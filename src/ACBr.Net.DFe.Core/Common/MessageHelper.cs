// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 20-01-2020
//
// Last Modified By : RFTD
// Last Modified On : 20-01-2020
// ***********************************************************************
// <copyright file="MessageHelper.cs" company="ACBr.Net">
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
using System.ServiceModel.Channels;
using System.Xml;
using ACBr.Net.Core;

namespace ACBr.Net.DFe.Core.Common
{
    public static class MessageHelper
    {
        /// <summary>
        /// Converte a message em uma Xml string.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string ToXml(ref Message message)
        {
            string messageXml;
            using (var sw = new ACBrStringWriter())
            using (var writer = XmlWriter.Create(sw))
            {
                message.WriteMessage(writer);
                writer.Flush();

                messageXml = sw.GetStringBuilder().ToString();
            }

            var reader = XmlReader.Create(new StringReader(messageXml));
            var copy = Message.CreateMessage(reader, int.MaxValue, message.Version);

            copy.Headers.Clear();
            copy.Headers.CopyHeadersFrom(message);

            copy.Properties.Clear();
            copy.Properties.CopyProperties(message.Properties);

            message.Close();
            message = copy;

            return messageXml;
        }

        public static void SaveXml(ref Message message, string file)
        {
            using (var fs = new FileStream(file, FileMode.CreateNew))
            {
                SaveXml(ref message, fs);
                fs.Flush();
            }
        }

        public static void SaveXml(ref Message message, Stream stream)
        {
            using (var sw = new ACBrStringWriter())
            using (var writer = XmlWriter.Create(stream))
            {
                message.WriteMessage(writer);
                writer.Flush();
            }

            stream.Position = 0;
            var reader = XmlReader.Create(stream);
            var copy = Message.CreateMessage(reader, int.MaxValue, message.Version);

            copy.Headers.Clear();
            copy.Headers.CopyHeadersFrom(message);

            copy.Properties.Clear();
            copy.Properties.CopyProperties(message.Properties);

            message.Close();
            message = copy;
        }
    }
}