namespace Mnk.TBox.Core.PluginsShared.Encoders
{
	public class JsonParser : CCodeFormatter
	{
		public JsonParser(): base('{', '}', ',')
		{
		}
	}
}
