using ACBr.Net.DFe.Core.Attributes;
using ACBr.Net.DFe.Core.Collection;
using ACBr.Net.DFe.Core.Document;
using ACBr.Net.DFe.Core.Serializer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ACBr.Net.DFe.Core.Tests
{
    [DFeRoot("RFTD")]
    public class TesteXml : DFeDocument<TesteXml>
    {
        public TesteXml()
        {
            XmlItems = new List<IXmlItem>();
            XmlProd = new DFeCollection<TesteXml4>();
            XmlProd2 = new DFeCollection<TesteXml4>();
            XmlProd3 = new TesteXml4[0];
            TesteListEnum = new DFeCollection<TesteEnum>();
            TesteDateTime = new DFeCollection<DateTime>();
            Xml5 = new TesteXml5();
            Signature = new DFeSignature();
        }

        [DFeAttribute(TipoCampo.Int, "id", Id = "AT1", Min = 2, Max = 2, Ocorrencia = Ocorrencia.NaoObrigatoria)]
        public int Id { get; set; }

        [DFeElement(TipoCampo.HorCFe, "dateTime1", Id = "DT1", Min = 0, Max = 19, Ocorrencia = Ocorrencia.NaoObrigatoria)]
        public DateTime TestDate { get; set; }

        [DFeElement(TipoCampo.DatHorTz, "dateTimeTz1", Id = "DTz1", Min = 0, Max = 19, Ocorrencia = Ocorrencia.NaoObrigatoria)]
        public DateTimeOffset TestDateTz { get; set; }

        [DFeElement(TipoCampo.De2, "decimal1", Id = "DC1", Min = 1, Max = 9, Ocorrencia = Ocorrencia.NaoObrigatoria)]
        public decimal TestDecimal { get; set; }

        [DFeElement(TipoCampo.Int, "nullInt", Id = "NI1", Min = 3, Max = 9, Ocorrencia = Ocorrencia.Obrigatoria)]
        public int? TestNullInt { get; set; }

        [DFeElement(TipoCampo.De2, "testString1", Id = "ST1", Min = 0, Max = 255, Ocorrencia = Ocorrencia.Obrigatoria)]
        public string TestString { get; set; }

        [DFeItem(typeof(TesteXml2), "Interface11")]
        [DFeItem(typeof(TesteXml3), "Interface12")]
        [DFeItem(typeof(Xml3Collection), "Interface13")]
        public IXmlItem TestInterface1 { get; set; }

        [DFeItem(typeof(TesteXml2), "Interface21")]
        [DFeItem(typeof(TesteXml3), "Interface22")]
        [DFeItem(typeof(Xml3Collection), "Interface23")]
        public IXmlItem TestInterface2 { get; set; }

        [DFeItem(typeof(TesteXml2), "Interface31")]
        [DFeItem(typeof(TesteXml3), "Interface32")]
        [DFeItem(typeof(Xml3Collection), "Interface33")]
        public IXmlItem TestInterface3 { get; set; }

        [DFeCollection("Itens")]
        [DFeItem(typeof(TesteXml2), "Item2")]
        [DFeItem(typeof(TesteXml3), "Item3")]
        public List<IXmlItem> XmlItems { get; set; }

        [DFeCollection("Itens2")]
        [DFeItem(typeof(TesteXml2), "Item2")]
        [DFeItem(typeof(TesteXml3), "Item3")]
        public IEnumerable<IXmlItem> XmlItems2 { get; set; }

        [DFeCollection("Itens3")]
        [DFeItem(typeof(TesteXml2), "Item2")]
        [DFeItem(typeof(TesteXml3), "Item3")]
        public IXmlItem[] XmlItems3 { get; set; }

        [DFeCollection("prod")]
        public DFeCollection<TesteXml4> XmlProd { get; set; }

        [DFeCollection("prod2")]
        public DFeCollection<TesteXml4> XmlProd2 { get; set; }

        [DFeCollection("prodArray")]
        public TesteXml4[] XmlProd3 { get; set; }

        [DFeCollection(TipoCampo.Enum, "TesteListEnum", MinSize = 1, MaxSize = 10, Min = 1, Max = 1, Ocorrencia = Ocorrencia.Obrigatoria)]
        public DFeCollection<TesteEnum> TesteListEnum { get; set; }

        [DFeCollection(TipoCampo.DatHor, "TesteDateTime", Min = 19, Max = 19, Ocorrencia = Ocorrencia.Obrigatoria)]
        public DFeCollection<DateTime> TesteDateTime { get; set; }

        [DFeElement(TipoCampo.Enum, "TesteEnum", Min = 1, Max = 1, Ocorrencia = Ocorrencia.Obrigatoria)]
        public TesteEnum TesteEnum { get; set; }

        [DFeElement(TipoCampo.Enum, "TesteEnum1", Min = 1, Max = 1, Ocorrencia = Ocorrencia.Obrigatoria)]
        public TesteEnum? TesteEnum1 { get; set; }

        [DFeElement(TipoCampo.Enum, "TesteEnum2", Min = 1, Max = 1, Ocorrencia = Ocorrencia.Obrigatoria)]
        public TesteEnum? TesteEnum2 { get; set; }

        public TesteXml5 Xml5 { get; set; }

        [DFeElement("TesteXML6", Ocorrencia = Ocorrencia.NaoObrigatoria)]
        public TesteXml2 Xml6 { get; set; }

        public DFeSignature Signature { get; set; }

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
                    var list = (IList)prop.GetValue(this, null);
                    var values = new ArrayList();
                    if (list != null)
                    {
                        values.AddRange(list);
                    }

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