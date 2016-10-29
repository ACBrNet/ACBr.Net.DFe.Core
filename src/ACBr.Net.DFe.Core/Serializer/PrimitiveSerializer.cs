// ***********************************************************************
// Assembly         : ACBr.Net.DFe.Core
// Author           : RFTD
// Created          : 05-04-2016
//
// Last Modified By : RFTD
// Last Modified On : 05-11-2016
// ***********************************************************************
// <copyright file="PrimitiveSerializer.cs" company="ACBr.Net">
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
using ACBr.Net.DFe.Core.Attributes;
using ACBr.Net.DFe.Core.Extensions;
using ACBr.Net.DFe.Core.Interfaces;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace ACBr.Net.DFe.Core.Serializer
{
	internal static class PrimitiveSerializer
	{
		#region Serialize

		/// <summary>
		/// Serializes a fundamental primitive object (e.g. string, int etc.) into a XElement using options.
		/// </summary>
		/// <param name="tag">The name of the primitive to serialize.</param>
		/// <param name="item">The item.</param>
		/// <param name="prop">The property.</param>
		/// <param name="options">Indicates how the output is formatted or serialized.</param>
		/// <returns>The XElement representation of the primitive.</returns>
		public static XObject Serialize(IDFeElement tag, object item, PropertyInfo prop, SerializerOptions options, int idx = -1)
		{
			try
			{
				var value = prop.GetValueOrIndex(item, idx);
				var estaVazio = value == null || value.ToString().IsEmpty();
				var conteudoProcessado = ProcessValue(ref estaVazio, tag.Tipo, value, tag.Ocorrencia, tag.Min, prop, item);

				string alerta;
				if (tag.Ocorrencia == Ocorrencia.Obrigatoria && estaVazio && tag.Min > 0)
				{
					alerta = SerializerOptions.ErrMsgVazio;
				}
				else
				{
					alerta = string.Empty;
				}

				if (conteudoProcessado.IsEmpty() && conteudoProcessado.Length < tag.Min && alerta.IsEmpty() &&
					conteudoProcessado.Length > 1)
				{
					alerta = SerializerOptions.ErrMsgMenor;
				}

				if (!string.IsNullOrEmpty(conteudoProcessado.Trim()) && conteudoProcessado.Length > tag.Max)
				{
					alerta = SerializerOptions.ErrMsgMaior;
				}

				if (!string.IsNullOrEmpty(alerta.Trim()) && SerializerOptions.ErrMsgVazio.Equals(alerta) && !estaVazio)
				{
					alerta += $" [{value}]";
				}

				options.WAlerta(tag.Id, tag.Name, tag.Descricao, alerta);

				XObject xmlTag = null;
				if (tag.Ocorrencia == Ocorrencia.Obrigatoria && estaVazio)
				{
					xmlTag = tag is DFeElementAttribute ? (XObject)new XElement(tag.Name) : new XAttribute(tag.Name, "");
				}

				if (estaVazio) return xmlTag;

				var retValue = options.RemoverAcentos ? conteudoProcessado.RemoveAccent() : conteudoProcessado;
				var ret = tag is DFeElementAttribute ? (XObject)new XElement(tag.Name, retValue) : new XAttribute(tag.Name, retValue);
				return ret;
			}
			catch (Exception ex)
			{
				options.WAlerta(tag.Id, tag.Name, tag.Descricao, ex.ToString());
				return null;
			}
		}

		private static string ProcessValue(ref bool estaVazio, TipoCampo tipo, object valor, Ocorrencia ocorrencia, int min, PropertyInfo prop, object item)
		{
			var conteudoProcessado = string.Empty;

			if (estaVazio) return conteudoProcessado;

			// ReSharper disable once SwitchStatementMissingSomeCases
			switch (tipo)
			{
				case TipoCampo.Str:
					conteudoProcessado = valor.ToString().Trim();
					break;

				case TipoCampo.Dat:
				case TipoCampo.DatCFe:
					DateTime data;
					if (DateTime.TryParse(valor.ToString(), out data))
					{
						conteudoProcessado = data.ToString(tipo == TipoCampo.DatCFe ? "yyyyMMdd" : "yyyy-MM-dd");
					}
					else
					{
						estaVazio = true;
					}
					break;

				case TipoCampo.Hor:
				case TipoCampo.HorCFe:
					DateTime hora;
					if (DateTime.TryParse(valor.ToString(), out hora))
					{
						conteudoProcessado = hora.ToString(tipo == TipoCampo.HorCFe ? "HHmmss" : "HH:mm:ss");
					}
					else
					{
						estaVazio = true;
					}
					break;

				case TipoCampo.DatHor:
					DateTime dthora;
					if (DateTime.TryParse(valor.ToString(), out dthora))
					{
						conteudoProcessado = dthora.ToString("s");
					}
					else
					{
						estaVazio = true;
					}
					break;

				case TipoCampo.DatHorTz:
					DateTime dthoratz;
					if (DateTime.TryParse(valor.ToString(), out dthoratz))
					{
						conteudoProcessado = dthoratz.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'sszzz");
					}
					else
					{
						estaVazio = true;
					}
					break;

				case TipoCampo.De2:
				case TipoCampo.De3:
				case TipoCampo.De4:
				case TipoCampo.De6:
				case TipoCampo.De10:
					decimal vDecimal;
					if (decimal.TryParse(valor.ToString(), out vDecimal))
					{
						if (ocorrencia == Ocorrencia.MaiorQueZero && vDecimal == 0)
						{
							estaVazio = true;
						}
						else
						{
							// ReSharper disable once SwitchStatementMissingSomeCases
							switch (tipo)
							{
								case TipoCampo.De2:
									conteudoProcessado = string.Format(CultureInfo.InvariantCulture, "{0:0.00}", vDecimal);
									break;

								case TipoCampo.De3:
									conteudoProcessado = string.Format(CultureInfo.InvariantCulture, "{0:0.000}", vDecimal);
									break;

								case TipoCampo.De4:
									conteudoProcessado = string.Format(CultureInfo.InvariantCulture, "{0:0.0000}", vDecimal);
									break;

								case TipoCampo.De6:
									conteudoProcessado = string.Format(CultureInfo.InvariantCulture, "{0:0.000000}", vDecimal);
									break;

								default:
									conteudoProcessado = string.Format(CultureInfo.InvariantCulture, "{0:0.0000000000}", vDecimal);
									break;
							}
						}
					}
					else
					{
						estaVazio = true;
					}
					break;

				case TipoCampo.Int:
					int vInt;
					if (int.TryParse(valor.ToString(), out vInt))
					{
						if (ocorrencia == Ocorrencia.MaiorQueZero && vInt == 0)
						{
							estaVazio = true;
						}
						else
						{
							conteudoProcessado = valor.ToString();
							if (conteudoProcessado.Length < min)
							{
								conteudoProcessado = conteudoProcessado.ZeroFill(min);
							}
						}
					}
					else
					{
						estaVazio = true;
					}
					break;

				case TipoCampo.StrNumberFill:
					conteudoProcessado = valor.ToString();
					if (conteudoProcessado.Length < min)
					{
						conteudoProcessado = conteudoProcessado.ZeroFill(min);
					}
					break;

				case TipoCampo.StrNumber:
					conteudoProcessado = valor.ToString().OnlyNumbers();
					break;

				case TipoCampo.Enum:
					var member = valor.GetType().GetMember(valor.ToString()).FirstOrDefault();
					var enumAttribute = member?.GetCustomAttributes(false).OfType<DFeEnumAttribute>().FirstOrDefault();
					var enumValue = enumAttribute?.Value;
					conteudoProcessado = enumValue ?? valor.ToString();
					break;

				case TipoCampo.Custom:
					var serialize = prop.GetSerializer(item);
					conteudoProcessado = serialize();
					break;

				default:
					conteudoProcessado = valor.ToString();
					break;
			}

			return conteudoProcessado;
		}

		#endregion Serialize

		#region Deserialize

		/// <summary>
		/// Deserializes the XElement to the fundamental primitive (e.g. string, int etc.) of a specified type using options.
		/// </summary>
		/// <param name="tag">The tag.</param>
		/// <param name="parentElement">The parent XElement used to deserialize the fundamental primitive.</param>
		/// <param name="item">The item.</param>
		/// <param name="prop">The property.</param>
		/// <param name="options">The options.</param>
		/// <returns>The deserialized fundamental primitive from the XElement.</returns>
		public static object Deserialize(IDFeElement tag, XObject parentElement, object item, PropertyInfo prop, SerializerOptions options, int idx = -1)
		{
			if (parentElement == null) return null;

			var element = parentElement as XElement;
			var value = element?.Value ?? ((XAttribute)parentElement).Value;
			return GetValue(tag.Tipo, value, item, prop);
		}

		private static object GetValue(TipoCampo tipo, string valor, object item, PropertyInfo prop)
		{
			if (valor.IsEmpty()) return null;

			object ret;
			// ReSharper disable once SwitchStatementMissingSomeCases
			switch (tipo)
			{
				case TipoCampo.Int:
					ret = valor.ToInt32();
					break;

				case TipoCampo.DatHor:
				case TipoCampo.DatHorTz:
					ret = valor.ToData();
					break;

				case TipoCampo.Dat:
				case TipoCampo.DatCFe:
					ret = DateTime.ParseExact(valor, tipo == TipoCampo.DatCFe ? "yyyyMMdd" : "yyyy-MM-dd", CultureInfo.InvariantCulture);
					break;

				case TipoCampo.Hor:
				case TipoCampo.HorCFe:
					ret = DateTime.ParseExact(valor, tipo == TipoCampo.HorCFe ? "HHmmss" : "HH:mm:ss", CultureInfo.InvariantCulture);
					break;

				case TipoCampo.De2:
				case TipoCampo.De3:
				case TipoCampo.De4:
				case TipoCampo.De10:
				case TipoCampo.De6:
					var numberFormat = CultureInfo.InvariantCulture.NumberFormat;
					ret = decimal.Parse(valor, numberFormat);
					break;

				case TipoCampo.Enum:
					var type = prop.PropertyType.IsGenericType ? prop.PropertyType.GetGenericArguments()[0] : prop.PropertyType;
					object value1 = type.GetMembers().Where(x => x.HasAttribute<DFeEnumAttribute>())
								 .SingleOrDefault(x => x.GetAttribute<DFeEnumAttribute>().Value == valor)?.Name ?? valor;

					ret = Enum.Parse(type, value1.ToString());
					break;

				case TipoCampo.Custom:
					var deserialize = prop.GetDeserializer(item);
					ret = deserialize(valor);
					break;

				default:
					ret = valor;
					break;
			}

			return ret;
		}

		#endregion Deserialize
	}
}