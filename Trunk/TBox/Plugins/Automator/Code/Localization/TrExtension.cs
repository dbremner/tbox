using Mnk.TBox.Locales.Localization.Plugins.Automator;

namespace Mnk.TBox.Plugins.Automator.Code.Localization
{
	public class TrExtension : Mnk.Library.WPFControls.Localization.TranslateExtension
	{
        public TrExtension(string key) : base(key, AutomatorLang.ResourceManager) { }
	}
}
