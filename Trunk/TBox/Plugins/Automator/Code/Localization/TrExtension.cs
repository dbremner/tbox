using Localization.Plugins.Automator;

namespace Automator.Code.Localization
{
	public class TrExtension : WPFControls.Localization.TranslateExtension
	{
        public TrExtension(string key) : base(key, AutomatorLang.ResourceManager) { }
	}
}
