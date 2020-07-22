using ACBr.Net.DFe.Core.Attributes;

namespace ACBr.Net.DFe.Core.Tests
{
    public enum DFeTipo
    {
        Envio,
        Consulta
    }

    public enum DFeVersao
    {
        [DFeEnum("2.00")]
        v200,

        [DFeEnum("3.00")]
        v300
    }
}