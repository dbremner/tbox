using Localization.Plugins.RegExpTester;

namespace RegExpTester.Code.Localization
{
	public class TrExtension : WPFControls.Localization.TranslateExtension
	{
        public TrExtension(string key) : base(key, RegExpTesterLang.ResourceManager) { }
	}
}
