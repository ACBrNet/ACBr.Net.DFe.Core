using ACBr.Net.DFe.Core.Serializer;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;

namespace ACBr.Net.DFe.Core.Tests
{
	public class SerializerTest
	{
		public static TesteXml GenerateXml()
		{
			var xml = new TesteXml
			{
				Id = 1,
				TestDate = DateTime.Now,
				TestDecimal = 100000M,
				TesteEnum = TesteEnum.Value3,
				TesteEnum1 = TesteEnum.Value1,
				TesteEnum2 = null,
				TestNullInt = 999
			};

			for (var i = 0; i < 3; i++)
			{
				var item = new TesteXml2
				{
					Id = i + 1,
					TestDecimal = xml.TestDecimal + i + 1,
					TestString = $"XmlItem2 {i + 1}"
				};
				xml.XmlItems.Add(item);
			}

			xml.XmlItems2 = xml.XmlItems.AsEnumerable();
			xml.XmlItems3 = xml.XmlItems.ToArray();

			for (var i = 0; i < 3; i++)
			{
				xml.TesteListEnum.Add((TesteEnum)i);
			}

			xml.TesteDateTime.Add(DateTime.Now);
			xml.TesteDateTime.Add(DateTime.MinValue);
			xml.TesteDateTime.Add(DateTime.MaxValue);

			for (var i = 0; i < 3; i++)
			{
				var item = new TesteXml3
				{
					Id = i + 1,
					TestDecimal = xml.TestDecimal + i + 1,
					TestString = $"XmlItem3 {i + 1}"
				};
				xml.XmlItems.Add(item);
			}

			var prod = xml.XmlProd.AddNew();
			prod.Id = 1;
			prod.TestDecimal = xml.TestDecimal + 1;
			prod.TestString = $"XmlItem4  1";

			xml.TestInterface1 = xml.XmlItems[0];
			xml.TestInterface2 = xml.XmlItems[1];

			return xml;
		}

		[Fact]
		public void TestSerializer()
		{
			var xml = GenerateXml();

			var serializer = DFeSerializer.CreateSerializer<TesteXml>();
			serializer.Serialize(xml, "teste.xml");

			Assert.True(File.Exists("teste.xml"), "Erro ao serializar a classe");

			var xmlDocument = XDocument.Load("teste.xml");
			Assert.NotNull(xmlDocument);

			Assert.NotNull(xmlDocument.Root);
			Assert.True(xmlDocument.Root?.Name == "RFTD", "Erro ao serializar root do Xml.");

			Assert.True(xmlDocument.Root.HasAttributes, "Erro ao serializar atributos do root.");
			Assert.True(xmlDocument.Root.Attributes().Count() == 1, "Erro ao serializar atributos do root.");
			Assert.True(xmlDocument.Root.FirstAttribute.Name == "id", "Erro ao serializar atributo id do root. Atributo com nome errado!");
			Assert.True(xmlDocument.Root.FirstAttribute.Value == "01", "Erro ao serializar atributo id do root. Valor incorreto!");

			var nodes = xmlDocument.Root.Nodes();
			Assert.True(nodes.Count() == 20, "Erro ao serializar dados do xml.");

			File.Delete("teste.xml");
		}

		[Fact]
		public void TestDeserializer()
		{
			var xml = GenerateXml();

			var serializer = DFeSerializer.CreateSerializer<TesteXml>();
			serializer.Serialize(xml, "teste.xml");

			var item = serializer.Deserialize("teste.xml");

			File.Delete("teste.xml");
		}
	}
}