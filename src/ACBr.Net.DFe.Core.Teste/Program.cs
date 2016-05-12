using System;
using ACBr.Net.DFe.Core.Serializer;

namespace ACBr.Net.DFe.Core.Teste
{
	class Program
	{
		static void Main(string[] args)
		{
			var xml = new TesteXml
			{
				Id = 1,
				TestDate = DateTime.Now,
				TestDecimal = 100000M,
				TesteEnum = TesteEnum.Value3,
				TestNullInt = 999
			};

			for (var i = 0; i < 3; i++)
			{
				var item = new TesteXml2
				{
					Id = i+1,
					TestDecimal = xml.TestDecimal + i + 1,
					TestString = $"XmlItem2 {i + 1}"
				};
				xml.XmlItems.Add(item);
			}

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

			for (var i = 0; i < 3; i++)
			{
				var item = xml.XmlProd.AddNew();
				item.Id = i + 1;
				item.TestDecimal = xml.TestDecimal + i + 1;
				item.TestString = $"XmlItem4  {i + 1}";
			}

			xml.TestInterface1 = xml.XmlItems[0];

			var serializer = DFeSerializer.CreateSerializer<TesteXml>();
			serializer.Serialize(xml, "testexml.xml");

			Console.WriteLine("XML Gerado com sucesso !");
			Console.ReadKey();
			var xmltest = serializer.Deserialize("testexml.xml");
			Console.WriteLine(xmltest.ToString());
			Console.ReadKey();
		}
	}
}
