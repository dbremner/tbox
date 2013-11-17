using Localization.Plugins.RequestsWatcher;

namespace RequestsWatcher.Code.Localization
{
	public class TrExtension : WPFControls.Localization.TranslateExtension
	{
        public TrExtension(string key) : base(key, RequestsWatcherLang.ResourceManager) { }
	}
}
