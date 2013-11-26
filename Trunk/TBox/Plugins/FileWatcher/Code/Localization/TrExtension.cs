using Localization.Plugins.FileWatcher;

namespace FileWatcher.Code.Localization
{
	public class TrExtension : WPFControls.Localization.TranslateExtension
	{
        public TrExtension(string key) : base(key, FileWatcherLang.ResourceManager) { }
	}
}
