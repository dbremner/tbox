using System.Resources;
using Localization.Plugins.TeamManager;

namespace TeamManager.Code.Localization
{
	public class TrExtension : WPFControls.Localization.TranslateExtension
	{
		public TrExtension(string key) : base(key){}

		protected override ResourceManager Manager
		{
            get { return TeamManagerLang.ResourceManager; }
		}
	}
}
