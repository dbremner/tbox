using Mnk.TBox.Locales.Localization.Plugins.RegExpTester;

namespace Mnk.TBox.Plugins.RegExpTester.Code.Localization
{
	public class TrExtension : Mnk.Library.WpfControls.Localization.TranslateExtension
	{
        public TrExtension(string key) : base(key, RegExpTesterLang.ResourceManager) { }
	}
}
