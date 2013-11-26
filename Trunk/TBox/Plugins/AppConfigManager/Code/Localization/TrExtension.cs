using Localization.Plugins.AppConfigManager;

namespace AppConfigManager.Code.Localization
{
	public class TrExtension : WPFControls.Localization.TranslateExtension
	{
        public TrExtension(string key) : base(key, AppConfigManagerLang.ResourceManager) { }
	}
}
