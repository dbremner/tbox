using System.Resources;
using Localization.Plugins.ProjectMan;

namespace ProjectMan.Code.Localization
{
	public class TrExtension : WPFControls.Localization.TranslateExtension
	{
		public TrExtension(string key) : base(key){}

		protected override ResourceManager Manager
		{
            get { return ProjectManLang.ResourceManager; }
		}
	}
}
