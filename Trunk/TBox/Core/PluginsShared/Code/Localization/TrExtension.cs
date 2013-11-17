using Localization.PluginsShared;

namespace PluginsShared.Code.Localization
{
	public class TrExtension : WPFControls.Localization.TranslateExtension
	{
        public TrExtension(string key) : base(key, PluginsSharedLang.ResourceManager) { }
	}
}
