using Mnk.TBox.Locales.Localization.Plugins.BookletPagesGenerator;

namespace Mnk.TBox.Plugins.BookletPagesGenerator.Code.Localization
{
	public class TrExtension : Mnk.Library.WPFControls.Localization.TranslateExtension
	{
        public TrExtension(string key) : base(key, BookletPagesGeneratorLang.ResourceManager) { }
	}
}
