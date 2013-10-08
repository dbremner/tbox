using System.Resources;
using Localization.Plugins.RequestsWatcher;

namespace RequestsWatcher.Code.Localization
{
	public class TrExtension : WPFControls.Localization.TranslateExtension
	{
		public TrExtension(string key) : base(key){}

		protected override ResourceManager Manager
		{
            get { return RequestsWatcherLang.ResourceManager; }
		}
	}
}
