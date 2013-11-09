using System.Resources;
using Localization.Plugins.Templates;

namespace Templates.Code.Localization
{
	public class TrExtension : WPFControls.Localization.TranslateExtension
	{
		public TrExtension(string key) : base(key){}

		protected override ResourceManager Manager
		{
            get { return TemplatesLang.ResourceManager; }
		}
	}
}
