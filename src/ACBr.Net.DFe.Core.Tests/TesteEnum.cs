using ACBr.Net.DFe.Core.Attributes;

namespace ACBr.Net.DFe.Core.Tests
{
	public enum TesteEnum
	{
		[DFeEnum("1")]
		Value1,

		[DFeEnum("2")]
		Value2,

		[DFeEnum("3")]
		Value3
	}
}