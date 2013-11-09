using System.Resources;
using Localization.Plugins.FileWatcher;

namespace FileWatcher.Code.Localization
{
	public class TrExtension : WPFControls.Localization.TranslateExtension
	{
		public TrExtension(string key) : base(key){}

		protected override ResourceManager Manager
		{
			get { return FileWatcherLang.ResourceManager; }
		}
	}
}
