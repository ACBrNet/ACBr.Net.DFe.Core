using System;
using ACBr.Net.DFe.Core.Serializer;

namespace ACBr.Net.DFe.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DFeItemValueAttribute : DFeBaseAttribute
    {
        #region Constructors

        public DFeItemValueAttribute(TipoCampo tipo = TipoCampo.Str)
        {
            Tipo = tipo;
        }

        #endregion Constructors
    }
}