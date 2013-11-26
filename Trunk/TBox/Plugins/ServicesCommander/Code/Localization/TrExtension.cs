using Localization.Plugins.ServicesCommander;

namespace ServicesCommander.Code.Localization
{
	public class TrExtension : WPFControls.Localization.TranslateExtension
	{
        public TrExtension(string key) : base(key, ServicesCommanderLang.ResourceManager) { }
	}
}
