namespace PluginsShared.Encoders
{
	public class JsonParser : CCodeFormatter
	{
		public JsonParser(): base('{', '}', ',')
		{
		}
	}
}
