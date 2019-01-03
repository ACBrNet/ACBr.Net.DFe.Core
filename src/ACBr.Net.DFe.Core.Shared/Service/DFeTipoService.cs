using ACBr.Net.DFe.Core.Attributes;

namespace ACBr.Net.DFe.Core.Service
{
    public enum DFeTipoService
    {
        [DFeEnum("CTe")]
        CTe,

        [DFeEnum("MDFe")]
        MDFe,

        [DFeEnum("NFe")]
        NFe,

        [DFeEnum("NFCe")]
        NFCe,

        [DFeEnum("NFSe")]
        NFSe
    }
}