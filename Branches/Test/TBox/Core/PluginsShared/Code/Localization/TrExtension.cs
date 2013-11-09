using System.Resources;
using Localization.PluginsShared;

namespace PluginsShared.Code.Localization
{
	public class TrExtension : WPFControls.Localization.TranslateExtension
	{
		public TrExtension(string key) : base(key){}

		protected override ResourceManager Manager
		{
            get { return PluginsSharedLang.ResourceManager; }
		}
	}
}
