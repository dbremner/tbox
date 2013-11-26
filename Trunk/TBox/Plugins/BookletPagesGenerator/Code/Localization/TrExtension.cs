using Localization.Plugins.BookletPagesGenerator;

namespace BookletPagesGenerator.Code.Localization
{
	public class TrExtension : WPFControls.Localization.TranslateExtension
	{
        public TrExtension(string key) : base(key, BookletPagesGeneratorLang.ResourceManager) { }
	}
}
