using System.Resources;
using Localization.Plugins.ProjectMan;

namespace ProjectMan.Code.Localization
{
	public class TrExtension : WPFControls.Localization.TranslateExtension
	{
        public TrExtension(string key) : base(key, ProjectManLang.ResourceManager) { }
	}
}
