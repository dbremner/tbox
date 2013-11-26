using Localization.Plugins.Searcher;

namespace Searcher.Code.Localization
{
	public class TrExtension : WPFControls.Localization.TranslateExtension
	{
        public TrExtension(string key) : base(key, SearcherLang.ResourceManager) { }
	}
}
