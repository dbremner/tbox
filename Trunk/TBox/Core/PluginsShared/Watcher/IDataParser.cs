using WPFControls.Code.Log;

namespace PluginsShared.Watcher
{
	public interface IDataParser
	{
		void Parse(string key, string name, string text, ICaptionedLog log);
	}
}
