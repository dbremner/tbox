using PluginsShared.Watcher;
using WPFControls.Code.Log;

namespace FileWatcher.Code
{
	class DataParser : IDataParser
	{
		public void Parse(string key, string name, string text, ICaptionedLog log)
		{
			log.Write(string.Format("{0}: '{1}'", key, name), text);
		}
	}
}
