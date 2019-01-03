using System;
using ACBr.Net.DFe.Core.Attributes;
using ACBr.Net.DFe.Core.Collection;
using ACBr.Net.DFe.Core.Common;
using ACBr.Net.DFe.Core.Serializer;

namespace ACBr.Net.DFe.Core.Service
{
    public class DFeWebserviceInfo<TTIpo> where TTIpo : Enum
    {
        #region Constructors

        public DFeWebserviceInfo()
        {
            Enderecos = new DFeCollection<DFeServiceAddresses<TTIpo>>();
        }

        #endregion Constructors

        #region Properties

        [DFeAttribute(TipoCampo.Enum, "Tipo")]
        public DFeTipoService Tipo { get; set; }

        [DFeAttribute(TipoCampo.Enum, "TipoEmissao")]
        public DFeTipoEmissao TipoEmissao { get; set; }

        [DFeCollection("Enderecos")]
        public DFeCollection<DFeServiceAddresses<TTIpo>> Enderecos { get; set; }

        #endregion Properties
    }
}