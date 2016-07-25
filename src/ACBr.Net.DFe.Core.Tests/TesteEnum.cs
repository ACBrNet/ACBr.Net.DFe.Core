using ACBr.Net.DFe.Core.Attributes;

namespace ACBr.Net.DFe.Core.Tests
{
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