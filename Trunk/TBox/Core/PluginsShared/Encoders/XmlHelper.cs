using System.Xml.Linq;

namespace PluginsShared.Encoders
{
	public static class XmlHelper
	{
		public static string Format(string text)
		{
			return XElement.Parse(text).ToString();
		}
	}
}
