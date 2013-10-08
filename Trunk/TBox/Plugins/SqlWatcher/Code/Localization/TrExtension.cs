using System.Resources;
using Localization.Plugins.SqlWatcher;

namespace SqlWatcher.Code.Localization
{
	public class TrExtension : WPFControls.Localization.TranslateExtension
	{
		public TrExtension(string key) : base(key){}

		protected override ResourceManager Manager
		{
            get { return SqlWatcherLang.ResourceManager; }
		}
	}
}
