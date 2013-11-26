using Localization.Plugins.TeamManager;

namespace TeamManager.Code.Localization
{
	public class TrExtension : WPFControls.Localization.TranslateExtension
	{
        public TrExtension(string key) : base(key, TeamManagerLang.ResourceManager) { }
	}
}
