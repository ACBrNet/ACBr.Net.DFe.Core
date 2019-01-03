using System;
using ACBr.Net.DFe.Core.Attributes;
using ACBr.Net.DFe.Core.Collection;
using ACBr.Net.DFe.Core.Document;

namespace ACBr.Net.DFe.Core.Service
{
    [DFeRoot("DFeServices", Namespace = "https://acbrnet.github.io")]
    public sealed class DFeService<TTIpo> : DFeDocument<DFeService<TTIpo>> where TTIpo : Enum
    {
        #region Constructors

        public DFeService()
        {
            Webservices = new DFeCollection<DFeWebserviceInfo<TTIpo>>();
        }

        #endregion Constructors

        #region Properties

        [DFeCollection("Services")]
        public DFeCollection<DFeWebserviceInfo<TTIpo>> Webservices { get; set; }

        #endregion Properties
    }
}