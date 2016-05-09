using System.Xml.Linq;

namespace ACBr.Net.DFe.Core.Extensions
{
	internal static class DocumentExtensions
	{
		public static void RemoveEmptyNamespace(this XDocument doc)
		{
			if (doc.Root == null)
				return;

			foreach (var node in doc.Root.Descendants())
			{
				if (node.Name.NamespaceName != "")
					continue;
				
				node.Attributes("xmlns").Remove();
				if (node.Parent != null)
					node.Name = node.Parent.Name.Namespace + node.Name.LocalName;
			}
		}
	}
}
