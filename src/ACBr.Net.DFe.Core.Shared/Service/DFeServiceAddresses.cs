using System;
using System.Collections.Generic;
using ACBr.Net.DFe.Core.Attributes;
using ACBr.Net.DFe.Core.Common;
using ACBr.Net.DFe.Core.Serializer;

namespace ACBr.Net.DFe.Core.Service
{
    public class DFeServiceAddresses<TTIpo> where TTIpo : Enum
    {
        #region Constructors

        public DFeServiceAddresses()
        {
            Endereco = new Dictionary<TTIpo, string>();
        }

        #endregion Constructors

        #region Properties

        [DFeAttribute(TipoCampo.Enum, "Ambiente")]
        public DFeTipoAmbiente Ambiente { get; set; }

        [DFeAttribute(TipoCampo.Enum, "UF")]
        public DFeCodUF UF { get; set; }

        [DFeDictionary("Enderecos")]
        [DFeDictionaryKey(TipoCampo.Enum, "Tipo", true)]
        [DFeDictionaryValue(TipoCampo.Str, "Endereco")]
        public Dictionary<TTIpo, string> Endereco { get; set; }

        #endregion Properties
    }
}