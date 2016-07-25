using ACBr.Net.DFe.Core.Attributes;
using ACBr.Net.DFe.Core.Collection;
using ACBr.Net.DFe.Core.Document;
using ACBr.Net.DFe.Core.Serializer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ACBr.Net.DFe.Core.Tests
{
	[DFeRoot("RFTD")]
	public class TesteXml
	{
		public TesteXml()
		{
			XmlItems = new List<IXmlItem>();
			XmlProd = new DFeCollection<TesteXml4>();
			Signature = new Signature();
		}

		[DFeAttribute(TipoCampo.Int, "id", Id = "AT1", Min = 2, Max = 2, Ocorrencias = 0)]
		public int Id { get; set; }

		[DFeElement(TipoCampo.HorCFe, "dateTime1", Id = "DT1", Min = 0, Max = 19, Ocorrencias = 0)]
		public DateTime TestDate { get; set; }

		[DFeElement(TipoCampo.De2, "decimal1", Id = "DC1", Min = 1, Max = 9, Ocorrencias = 0)]
		public decimal TestDecimal { get; set; }

		[DFeElement(TipoCampo.Int, "nullInt", Id = "NI1", Min = 3, Max = 9, Ocorrencias = 1)]
		public int? TestNullInt { get; set; }

		[DFeElement(TipoCampo.De2, "testString1", Id = "ST1", Min = 0, Max = 255, Ocorrencias = 1)]
		public string TestString { get; set; }

		[DFeItem(typeof(TesteXml2), "Interface1")]
		[DFeItem(typeof(TesteXml3), "Interface2")]
		public IXmlItem TestInterface1 { get; set; }

		[DFeItem(typeof(TesteXml2), "Interface1")]
		[DFeItem(typeof(TesteXml3), "Interface2")]
		public IXmlItem TestInterface2 { get; set; }

		[DFeElement("Itens")]
		[DFeItem(typeof(TesteXml2), "Item2")]
		[DFeItem(typeof(TesteXml3), "Item3")]
		public List<IXmlItem> XmlItems { get; set; }

		[DFeElement("prod")]
		public DFeCollection<TesteXml4> XmlProd { get; set; }

		[DFeElement(TipoCampo.Enum, "TesteEnum", Min = 1, Max = 1, Ocorrencias = 1)]
		public TesteEnum TesteEnum { get; set; }

		public Signature Signature { get; set; }

		private bool ShouldSerializeId()
		{
			return Id > 0;
		}

		#region Overrides

		public override string ToString()
		{
			var props = GetType().GetProperties();
			var builder = new StringBuilder();
			foreach (var prop in props)
			{
				if (prop.PropertyType.IsArray || prop.PropertyType.IsAssignableFrom(typeof(IEnumerable)) ||
					(prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(List<>)) ||
					(prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(DFeCollection<>)))
				{
					var values = ((IEnumerable<object>)prop.GetValue(this, null) ?? new object[0]).ToArray();
					foreach (var value in values)
						builder.AppendLine($"{prop.Name}: {value.GetType()}{Environment.NewLine}{value}");
				}
				else
				{
					var value = prop.GetValue(this, null);
					builder.AppendLine($"{prop.Name}: {value}");
				}
			}

			return builder.ToString();
		}

		#endregion Overrides
	}
}