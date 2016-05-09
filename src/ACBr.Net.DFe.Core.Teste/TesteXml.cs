using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ACBr.Net.DFe.Core.Attributes;
using ACBr.Net.DFe.Core.Collection;
using ACBr.Net.DFe.Core.Document;
using ACBr.Net.DFe.Core.Serializer;

namespace ACBr.Net.DFe.Core.Teste
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

		[DFeAttribute(TipoCampo.Int, "AT1", "id", 2, 2, 0)]
		public int Id { get; set; }

		[DFeElement(TipoCampo.HorCFe, "DT1", "dateTime1", 0, 19, 0)]
		public DateTime TestDate { get; set; }

		[DFeElement(TipoCampo.De2, "DC1", "decimal1", 0, 9, 0)]
		public decimal TestDecimal { get; set; }

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

	public class TesteXml2 : IXmlItem
	{
		[DFeAttribute(TipoCampo.Int, "AT2", "id", 2, 2, 0)]
		public int Id { get; set; }

		[DFeElement(TipoCampo.Custom, "ST1", "custom1", 0, 19, 0)]
		public string TestString { get; set; }

		[DFeElement(TipoCampo.De3, "DC2", "decimal2", 0, 9, 0)]
		public decimal TestDecimal { get; set; }

		private object SerializeTestString()
		{
			return $"{TestString} || SerializeTestString {Id:00}";
		}

		private object DeserializeTestString(string value)
		{
			return value.Replace($" || SerializeTestString {Id:00}", string.Empty);
		}

		#region Overrides

		public override string ToString()
		{
			var props = this.GetType().GetProperties();
			var builder = new StringBuilder();
			foreach (var prop in props)
			{
				if (prop.PropertyType.IsArray || prop.PropertyType.IsAssignableFrom(typeof(IEnumerable))
				    || prop.PropertyType.IsAssignableFrom(typeof(ICollection)) ||
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

	public class TesteXml3 : IXmlItem
	{
		[DFeAttribute(TipoCampo.Int, "AT2", "id", 2, 2, 0)]
		public int Id { get; set; }

		[DFeElement(TipoCampo.Custom, "ST1", "custom1", 0, 19, 0)]
		public string TestString { get; set; }

		[DFeElement(TipoCampo.De3, "DC2", "decimal2", 0, 9, 0)]
		public decimal TestDecimal { get; set; }

		private object SerializeTestString()
		{
			return $"{TestString} || SerializeTestString {Id:00}";
		}

		private object DeserializeTestString(string value)
		{
			return value.Replace($" || SerializeTestString {Id:00}", string.Empty);
		}

		#region Overrides

		public override string ToString()
		{
			var props = GetType().GetProperties();
			var builder = new StringBuilder();
			foreach (var prop in props)
			{
				if (prop.PropertyType.IsArray || prop.PropertyType.IsAssignableFrom(typeof(IEnumerable))
					|| prop.PropertyType.IsAssignableFrom(typeof(ICollection)) ||
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

	public class TesteXml4
	{
		[DFeAttribute(TipoCampo.Int, "AT2", "id", 2, 2, 0)]
		public int Id { get; set; }

		[DFeElement(TipoCampo.Custom, "ST1", "custom1", 0, 19, 0)]
		public string TestString { get; set; }

		[DFeElement(TipoCampo.De3, "DC2", "decimal2", 0, 9, 0)]
		public decimal TestDecimal { get; set; }

		private object SerializeTestString()
		{
			return $"{TestString} || SerializeTestString {Id:00}";
		}

		private object DeserializeTestString(string value)
		{
			return value.Replace($" || SerializeTestString {Id:00}", string.Empty);
		}

		#region Overrides

		public override string ToString()
		{
			var props = GetType().GetProperties();
			var builder = new StringBuilder();
			foreach (var prop in props)
			{
				if (prop.PropertyType.IsArray || prop.PropertyType.IsAssignableFrom(typeof(IEnumerable))
					|| prop.PropertyType.IsAssignableFrom(typeof(ICollection)) ||
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

	public interface IXmlItem
	{
		
	}

	public enum TesteEnum
	{
		[DFeEnum("1")]
		Value1,
		[DFeEnum("1")]
		Value2,
		[DFeEnum("1")]
		Value3
	}
}