using System.Resources;
using Localization.Plugins.Searcher;

namespace Searcher.Code.Localization
{
	public class TrExtension : WPFControls.Localization.TranslateExtension
	{
		public TrExtension(string key) : base(key){}

		protected override ResourceManager Manager
		{
            get { return SearcherLang.ResourceManager; }
		}
	}
}
