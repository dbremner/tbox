using Mnk.TBox.Locales.Localization.Plugins.RequestsWatcher;

namespace Mnk.TBox.Plugins.RequestsWatcher.Code.Localization
{
	public class TrExtension : Mnk.Library.WPFControls.Localization.TranslateExtension
	{
        public TrExtension(string key) : base(key, RequestsWatcherLang.ResourceManager) { }
	}
}
